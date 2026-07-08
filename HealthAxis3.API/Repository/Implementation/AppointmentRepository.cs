using HealthAxis3.API.Data;
using HealthAxis3.API.Events;
using HealthAxis3.API.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class AppointmentRepository(AppDbContext context,IBus bus) : Repository<Appointment>(context), IAppointmentRepository
    {
        public async Task<Appointment> CreatesAsync(Appointment appointment, CancellationToken ct = default)
        {
            await context.Set<Appointment>().AddAsync(appointment, ct);
            await context.SaveChangesAsync(ct);
            await bus.Publish(new AppointmentBookedEvent
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = appointment.Patient.PatientName,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.Slot
            });

            return appointment;
        }
        public async Task<List<Appointment>> GetByDoctorIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await context.Set<Appointment>().Where(e => e.DoctorId == id).ToListAsync(ct);
            return existing;
        }
        public async Task<List<Appointment>> GetByPatientIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await context.Set<Appointment>().Where(e => e.PatientId == id).ToListAsync(ct);
            return existing;
        }


        public async Task<Appointment?> GetWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == id, ct);
        }

        public async Task<List<Appointment>> GetExpiredCancelledAsync(CancellationToken ct = default)
        {
            return await context.Appointments
                .Where(a => a.Status == "Cancelled" &&
                            a.ScheduledDate < DateTime.Now)
                .ToListAsync(ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var appointment = await context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == id, ct);
            if (appointment == null)
            {
                return false;
            }
            else if (appointment.Status != "Cancelled")
            {
                return false;
            }
            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync(ct);
            return true;
        }
    }
}