using HealthAxis3.BlazorUI;
using HealthAxis3.BlazorUI.Auth;
using HealthAxis3.BlazorUI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7120/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
{
    return sp.GetRequiredService<CustomAuthenticationStateProvider>();
});
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DoctorService>();
await builder.Build().RunAsync();
