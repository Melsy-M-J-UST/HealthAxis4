using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Service.Implementation;
using Moq;
using HealthAxis3.Shared.Models.Dtos.AppointmentDtos;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;

namespace HealthAxis3.Tests.ServiceTests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _service;
        private readonly AppointmentDto dto;
        private readonly Appointment entity;
        private readonly List<Appointment> list;
        private readonly List<AppointmentDto> dtoList;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AppointmentService(_repoMock.Object, _mapperMock.Object);

            dto = new AppointmentDto { AppointmentId = 1, Doctor= new DoctorDto { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient= new PatientDto { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot="09:00 AM" };
            entity = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM" };
            list = [new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM" }];
            dtoList = [new AppointmentDto { AppointmentId = 1, Doctor = new DoctorDto { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new PatientDto { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM" }];
        }
        [Fact]
        public async Task AddAsync_Should_Map_And_ReturnDto()
        {
            _mapperMock.Setup(m => m.Map<Appointment>(dto)).Returns(entity);
            _repoMock.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<AppointmentDto>(entity)).Returns(dto);

            var result = await _service.AddAsync(dto);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllAsync_Should_ReturnList()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
                       .Returns(dtoList);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
        }
        [Fact]
        public async Task GetByDoctorIdAsync_Should_ReturnList()
        {
            _repoMock.Setup(r => r.GetByDoctorIdAsync(1)).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
                       .Returns(dtoList);

            var result = await _service.GetByDoctorIdAsync(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetByIdAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<AppointmentDto>(It.IsAny<Appointment>()))
                       .Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetByPatientIdAsync_Should_ReturnList()
        {
            _repoMock.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
                       .Returns(dtoList);

            var result = await _service.GetByPatientIdAsync(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateStatus_Should_Return_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Appointment?)null);

            var result = await _service.UpdateAppointmentStatus(1, "Confirmed");

            Assert.Equal("Appointment not found", result);
        }
        [Fact]
        public async Task UpdateStatus_Should_Return_StatusNotAllowed_WhenCancelled()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Cancelled" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.UpdateAppointmentStatus(1, "Confirmed");

            Assert.Equal("Status cannot be changed", result);
        }
        [Fact]
        public async Task UpdateStatus_Cancel_WithoutReason()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Pending" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.UpdateAppointmentStatus(1, "Cancelled");

            Assert.Equal("Cancellation reason required", result);
        }
        [Fact]
        public async Task UpdateStatus_InvalidTransition()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Pending" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.UpdateAppointmentStatus(1, "Completed");

            Assert.Equal("Invalid transition", result);
        }
        [Fact]
        public async Task UpdateStatus_Complete_BeforeDate()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Confirmed", ScheduledDate = DateTime.Now.AddDays(1)};

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.UpdateAppointmentStatus(1, "Completed");

            Assert.Equal("Cannot complete before appointment date", result);
        }
        [Fact]
        public async Task UpdateStatus_Complete_Success()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Confirmed", ScheduledDate = DateTime.Now.AddDays(-1)};

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.UpdateAppointmentStatus(1, "Completed");

            Assert.Equal("REDIRECT_TO_HEALTH_RECORD", result);
            _repoMock.Verify(r => r.UpdateAsync(1, appt), Times.Once);
        }
        [Fact]
        public async Task UpdateStatus_Confirmed_To_Cancelled()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Confirmed" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.UpdateAppointmentStatus(1, "Cancelled", "Reason");

            Assert.Equal("Status updated successfully", result);
        }
        [Fact]
        public async Task DeleteAppointment_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment?)null);

            var result = await _service.DeleteAppointment(1);

            Assert.Equal("Appointment not found", result);
        }
        [Fact]
        public async Task DeleteAppointment_NotCancelled()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Confirmed" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.DeleteAppointment(1);

            Assert.Equal("Only cancelled appointments can be deleted", result);
        }
        [Fact]
        public async Task DeleteAppointment_BeforeDate()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Cancelled", ScheduledDate = DateTime.Now.AddDays(1) };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.DeleteAppointment(1);

            Assert.Equal("Cannot delete before appointment date", result);
        }
        [Fact]
        public async Task DeleteAppointment_Success()
        {
            var appt = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status = "Cancelled", ScheduledDate = DateTime.Now.AddDays(-1), CancellationReason="Busy" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appt);

            var result = await _service.DeleteAppointment(1);

            Assert.Equal("Cancelled appointment deleted", result);
            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
        [Fact]
        public async Task CleanupCancelledAppointments_Should_Delete_All()
        {
            var list = new List<Appointment>
            {
                new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM", Status="Cancelled", CancellationReason="Busy"},
                new Appointment { AppointmentId = 2, Doctor = new Doctor { DoctorId = 2, DoctorName = "Joy Prakash", Specialisation = "Psychologist" }, Patient = new Patient { PatientId = 2, PatientName = "Arunima", Email = "arunima@test.com", Gender = "Female", PhoneNumber = "8744336554", DateOfBirth = DateTime.Today.AddYears(-27) }, Slot = "09:00 AM", CancellationReason="Rescheduled" }
            };

            _repoMock.Setup(r => r.GetExpiredCancelledAsync()).ReturnsAsync(list);

            await _service.CleanupCancelledAppointments();

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
            _repoMock.Verify(r => r.DeleteAsync(2), Times.Once);
        }
    }
}
