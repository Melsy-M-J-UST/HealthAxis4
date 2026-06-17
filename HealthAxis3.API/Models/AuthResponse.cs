namespace HealthAxis3.API.Models
{
    public class AuthResponse
    {
        public string Accesstoken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
