using Xunit;
using Moq;
using AutoMapper;
using HealthAxis3.API.Service.Implementation;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;
namespace HealthAxis3.Tests.ServiceTests
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        private readonly PatientDto dto;
        private readonly PatientCreateDto dtoCreate;
        private readonly Patient entity;
        private readonly List<Patient> entities;
        private readonly List<PatientDto> dtos;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PatientService(_repoMock.Object, _mapperMock.Object);
            dto = new PatientDto { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) };
            dtoCreate = new PatientCreateDto { PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) };
            entity = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) };

            entities = new List<Patient> { new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) } };

            dtos = new List<PatientDto> { new PatientDto { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) } };

        }
        [Fact]
        public async Task AddAsync_Should_Map_Save_And_ReturnDto()
        {
            _mapperMock.Setup(m => m.Map<Patient>(dtoCreate))
                       .Returns(entity);

            _repoMock
                .Setup(r => r.CreateAsync(It.IsAny<Patient>()))
                .ReturnsAsync(entity);

            _mapperMock.Setup(m => m.Map<PatientDto>(entity))
                       .Returns(dto);

            var result = await _service.AddAsync(dtoCreate);

            _mapperMock.Verify(m => m.Map<Patient>(dtoCreate), Times.Once);
            _repoMock.Verify(r => r.CreateAsync(It.IsAny<Patient>()), Times.Once);
            _mapperMock.Verify(m => m.Map<PatientDto>(entity), Times.Once);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeactivatePatientAsync_Should_ReturnMappedDto()
        {

            _repoMock.Setup(r => r.DeactivateAsync(1)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<PatientDto>(entity)).Returns(dto);

            var result = await _service.DeactivatePatientAsync(1);

            _repoMock.Verify(r => r.DeactivateAsync(1), Times.Once);
            _mapperMock.Verify(m => m.Map<PatientDto>(entity), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
        }
        [Fact]
        public async Task GetAllAsync_Should_ReturnListOfDtos()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<PatientDto>>(entities)).Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
        }
        [Fact]
        public async Task GetByIdAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<PatientDto>(entity)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.Equal(1, result.PatientId);
        }
        [Fact]
        public async Task GetByNameAsync_Should_ReturnDtos()
        {
            _repoMock.Setup(r => r.GetByNameAsync("Arun")).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<PatientDto>>(entities)).Returns(dtos);

            var result = await _service.GetByNameAsync("Arun");

            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task GetByPhoneAsync_Should_ReturnDtos()
        {
            _repoMock.Setup(r => r.GetByPhoneAsync("123")).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<PatientDto>>(entities)).Returns(dtos);

            var result = await _service.GetByPhoneAsync("123");

            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task UpdateAsync_Should_SetId_Map_Update_And_ReturnDto()
        {
            var updatedEntity = new Patient { PatientId = 1, PatientName = "Arun", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356654", DateOfBirth = DateTime.Today.AddYears(-25) };
            var resultDto = new PatientDto { PatientId = 1, PatientName = "Arun Kumar", Email = "arun@test.com", Gender = "Male", PhoneNumber = "8744356674", DateOfBirth = DateTime.Today.AddYears(-25) };

            _mapperMock.Setup(m => m.Map<Patient>(dto)).Returns(entity);
            _repoMock.Setup(r => r.UpdateAsync(1, It.IsAny<Patient>()))
                     .ReturnsAsync(updatedEntity);
            _mapperMock.Setup(m => m.Map<PatientDto>(updatedEntity))
                       .Returns(resultDto);

            var result = await _service.UpdateAsync(1, dto);

            Assert.Equal(1, entity.PatientId);
            Assert.Equal(1, result.PatientId);

            _repoMock.Verify(r => r.UpdateAsync(1, entity), Times.Once);
        }
    }
}