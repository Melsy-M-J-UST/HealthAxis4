using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using AutoMapper;
using HealthAxis3.API.Service.Implementation;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.HealthrecordDto;


namespace Healthaxis3.Tests.ServiceTests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _service;
        private readonly HealthRecordDto dto;
        private readonly HealthRecord entity;
        private readonly List<HealthRecord> list;
        private readonly List<HealthRecordDto> dtoList;

        public HealthRecordServiceTests()
        {
            _repoMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new HealthRecordService(_repoMock.Object, _mapperMock.Object);

            dto = new HealthRecordDto { Appointment = new Appointment{ AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM" }, Doctor= new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Diagnosis="Fever", Prescription="Paracetamol" };
            entity = new HealthRecord { Appointment = new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Slot = "09:00 AM" }, Doctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" }, Patient = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) }, Diagnosis = "Fever", Prescription = "Paracetamol" };
            list = new List<HealthRecord> { entity };
            dtoList = new List<HealthRecordDto> { dto };
        }
        [Fact]
        public async Task AddAsync_Should_Map_Save_And_ReturnDto()
        {
            _mapperMock.Setup(m => m.Map<HealthRecord>(dto)).Returns(entity);
            _repoMock.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<HealthRecordDto>(entity)).Returns(dto);

            var result = await _service.AddAsync(dto);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllAsync_Should_ReturnList()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                       .Returns(dtoList);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
        }
        [Fact]
        public async Task GetByIdAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<HealthRecordDto>(It.IsAny<HealthRecord>()))
                       .Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetByDoctorIdAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByDoctorIdAsync(1)).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>())).Returns(dtoList);

            var result = await _service.GetByDoctorIdAsync(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetByPatientIdAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>())).Returns(dtoList);

            var result = await _service.GetByPatientIdAsync(1);

            Assert.NotNull(result);
        }

    }
}
