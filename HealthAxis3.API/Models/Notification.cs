namespace HealthAxis3.API.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
