namespace HealthAxis3.API.Events
{
    public class AppointmentBookedEvent
    {
        public string EventType { get; set; } = "AppointmentBooked";
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = "00:00 AM";
    }
}
