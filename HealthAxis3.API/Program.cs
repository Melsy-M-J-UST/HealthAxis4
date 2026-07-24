using HealthAxis3.API.Data;
using HealthAxis3.API.Mappings;
using HealthAxis3.API.Models;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Repository.Implementation;
using HealthAxis3.API.Service;
using HealthAxis3.API.Service.Background;
using HealthAxis3.API.Service.Implementation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json;
using Serilog;
using HealthAxis3.API.Options;
using HealthAxis3.API.Messaging;
using Microsoft.AspNetCore.StaticFiles;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("HealthAxis Api Starting ..... ");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => {
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services).Enrich.FromLogContext().WriteTo.Console().WriteTo.File("logs/healthaxis-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7);
});
// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(options =>

options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DbCon"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 4;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    var jwt = builder.Configuration.GetSection("Jwt");
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwt["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwt["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "HealthAxis API",
            Version = "v1",
            Description = "Healthcare Appointment Management API"
        });

    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "bearer",
                document)] = []
        });
});
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHostedService<HeartbeatBackgroundService>();
var rabbitMQConfiguration = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AppointmentBookedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMQConfiguration["HostName"], rabbitMQConfiguration["VirtualHost"], h =>
        {
            h.Username(rabbitMQConfiguration["UserName"]!);
            h.Password(rabbitMQConfiguration["Password"]!);
        });
        cfg.ReceiveEndpoint(rabbitMQConfiguration["AppointmentQueue"]!, e =>
        {
            e.ConfigureConsumer<AppointmentBookedConsumer>(context);
        });
        //cfg.ConfigureEndpoints(context);
    });
});
//builder.Services.Configure<GarnetOptions>(builder.Configuration.GetSection("Garnet"));
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    var garnetOptions = builder.Configuration.GetSection("Garnet").Get<GarnetOptions>()?? new GarnetOptions();
//    options.Configuration = garnetOptions.ConnectionString;
//    options.InstanceName =garnetOptions.InstanceName;
//});
builder.Services.AddCors(p =>
{
    p.AddPolicy("CorsPolicy", cfg =>
    {
        cfg.WithOrigins("https://localhost:7113","https://localhost:64647").AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

var app = builder.Build();
app.UseSerilogRequestLogging();
using (var scope = app.Services.CreateScope())
{

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRoleAsync(roleManager);

}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".data"] = "application/octet-stream";
contentTypeProvider.Mappings[".wasm"] = "application/wasm";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider
});
app.UseCors("AllowBlazor");
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/angular/", async context =>
{
    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath,"angular", "index.html"));
});
app.MapGet("/angular/{*path:nonfile}", async context =>
{
    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "angular", "index.html"));
});
app.MapGet("/blazor/", async context =>
{
    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "blazor", "index.html"));
});
app.MapGet("/blazor/{*path:nonfile}", async context =>
{
    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "blazor", "index.html"));
});
app.UseSerilogRequestLogging();
await app.RunAsync();
