using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models.Dtos.AppointmentDtos;
using HealthAxis3.API.Repository;

namespace HealthAxis3.API.Service.Implementation
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> AddAsync(AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var savedEntity = await repository.CreateAsync(appointment);
            return mapper.Map<AppointmentDto>(savedEntity);
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAllAsync());
        }

        public async Task<List<AppointmentDto>> GetByDoctorIdAsync(int id, CancellationToken ct = default)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetByDoctorIdAsync(id));
        }

        public async Task<AppointmentDto> GetByIdAsync(int id, CancellationToken ct= default)
        {
            return mapper.Map<AppointmentDto>(await repository.GetByIdAsync(id));
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(int id, CancellationToken ct = default)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetByPatientIdAsync(id));
        }

        public async Task<string> UpdateAppointmentStatus(int id, string status, string? reason = null)
        {
            var appointment = await repository.GetByIdAsync(id);
            if (appointment == null)
                return "Appointment not found";
            var currentStatus = appointment.Status;
            if (currentStatus == "Cancelled" || currentStatus == "Completed")
                return "Status cannot be changed";
            if (currentStatus == "Pending")
            {
                if (status == "Confirmed")
                {
                    appointment.Status = "Confirmed";
                }
                else if (status == "Cancelled")
                {
                    if (string.IsNullOrWhiteSpace(reason))
                        return "Cancellation reason required";
                    appointment.Status = "Cancelled";
                    appointment.CancellationReason = reason;
                }
                else return "Invalid transition";
            }
            else if (currentStatus == "Confirmed")
            {
                if (status == "Completed")
                {
                    if (DateTime.Now < appointment.ScheduledDate)
                        return "Cannot complete before appointment date";
                    appointment.Status = "Completed";
                    await repository.UpdateAsync(id, appointment);
                    return "REDIRECT_TO_HEALTH_RECORD";
                }
                else if (status == "Cancelled")
                {
                    if (string.IsNullOrWhiteSpace(reason))
                        return "Cancellation reason required";
                    appointment.Status = "Cancelled";
                    appointment.CancellationReason = reason;
                }
                else return "Invalid transition";
            }
            await repository.UpdateAsync(id, appointment);
            return "Status updated successfully";
        }

        public async Task<string> DeleteAppointment(int id)
        {
            var appointment = await repository.GetByIdAsync(id);

            if (appointment == null)
                return "Appointment not found";

            if (appointment.Status == "Cancelled")
            {
                if (DateTime.Now >= appointment.ScheduledDate)
                {
                    await repository.DeleteAsync(id);
                    return "Cancelled appointment deleted";
                }
                return "Cannot delete before appointment date";
            }

            return "Only cancelled appointments can be deleted";
        }

        public async Task CleanupCancelledAppointments()
        {
            var expired = await repository.GetExpiredCancelledAsync();

            foreach (var appt in expired)
            {
                await repository.DeleteAsync(appt.AppointmentId);
            }
        }
    }
}