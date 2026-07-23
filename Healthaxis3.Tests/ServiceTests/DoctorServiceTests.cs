using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Service.Implementation;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Xunit;

namespace HealthAxis3.Tests.ServiceTests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly DoctorService _service;

        private readonly DoctorDto _dto;
        private readonly Doctor _entity;
        private readonly List<Doctor> entities;
        private readonly List<DoctorDto> dtos;
        private readonly DoctorUpdateDto updateDto;
        private readonly Doctor updatedDoctor;
        private readonly DoctorUpdateDto resultDto;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ILogger<DoctorService>> _loggerMock;

        public DoctorServiceTests()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null!, // IOptions<IdentityOptions>
                null!, // IPasswordHasher<ApplicationUser>
                null!, // IEnumerable<IUserValidator<ApplicationUser>>
                null!, // IEnumerable<IPasswordValidator<ApplicationUser>>
                null!, // ILookupNormalizer
                null!, // IdentityErrorDescriber
                null!, // IServiceProvider
                null!  // ILogger<UserManager<ApplicationUser>>
            );
            _loggerMock = new Mock<ILogger<DoctorService>>();
            _cacheMock = new Mock<IDistributedCache>();
            _service = new DoctorService(_repoMock.Object, _mapperMock.Object, _userManagerMock.Object, _cacheMock.Object, _loggerMock.Object);

            _dto = new DoctorDto { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" };
            _entity = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" };
            entities = new List<Doctor> { new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" } };
            dtos = new List<DoctorDto> { new DoctorDto { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Cardiologist" } };
            updateDto = new DoctorUpdateDto { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Neurologist" };
            updatedDoctor = new Doctor { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Neurologist" };
            resultDto = new DoctorUpdateDto { DoctorId = 1, DoctorName = "Meera Varma", Specialisation = "Neurologist" };
        }
        [Fact]
        public async Task AddAsync_Should_Map_Save_And_ReturnDto()
        {
            _mapperMock.Setup(m => m.Map<Doctor>(_dto)).Returns(_entity);
            _repoMock.Setup(r => r.CreateAsync(_entity)).ReturnsAsync(_entity);
            _mapperMock.Setup(m => m.Map<DoctorDto>(_entity)).Returns(_dto);

            var result = await _service.AddAsync(_dto);

            _mapperMock.Verify(m => m.Map<Doctor>(_dto), Times.Once);
            _repoMock.Verify(r => r.CreateAsync(_entity), Times.Once);
            _mapperMock.Verify(m => m.Map<DoctorDto>(_entity), Times.Once);
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "Doctor@123"), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync("doctors:all", default),Times.Once);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllAsync_Should_ReturnListOfDtos()
        {
            _cacheMock.Setup(c => c.GetStringAsync(It.IsAny<string>(), default)).ReturnsAsync((string?)null);
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(entities)).Returns(dtos);
            var result = await _service.GetAllAsync();
            Assert.Single(result);
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task GetByIdAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_entity);
            _mapperMock.Setup(m => m.Map<DoctorDto>(_entity)).Returns(_dto);

            var result = await _service.GetByIdAsync(1);

            Assert.Equal(1, result.DoctorId);
        }
        [Fact]
        public async Task GetByNameAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetByNameAsync("Meera Varma")).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(entities)).Returns(dtos);

            var result = await _service.GetByNameAsync("Meera Varma");

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetBySpecialisationAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.GetBySpecialisationAsync("Cardio")).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(entities)).Returns(dtos);

            var result = await _service.GetBySpecialisationAsync("Cardio");

            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateAsync_Should_Map_SetId_Update_And_ReturnDto()
        {
            _mapperMock.Setup(m => m.Map<Doctor>(updateDto)).Returns(_entity);
            _repoMock.Setup(r => r.UpdateAsync(1, _entity)).ReturnsAsync(updatedDoctor);
            _mapperMock.Setup(m => m.Map<DoctorUpdateDto>(updatedDoctor)).Returns(resultDto);

            var result = await _service.UpdateAsync(1, updateDto);

            Assert.Equal(1, _entity.DoctorId); // verifies assignment
            _repoMock.Verify(r => r.UpdateAsync(1, _entity), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync("doctors:all", default),Times.Once);
        }
        [Fact]
        public async Task GetAvailableSlots_Should_ReturnSlots()
        {
            var slots = new List<string> { "10:00", "11:00" };
            var date = DateTime.Today;

            _repoMock.Setup(r => r.GetDoctorAvailability(1, date)).ReturnsAsync(slots);

            var result = await _service.GetAvailableSlots(1, date);

            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async Task DeactivateDoctorAsync_Should_ReturnDto()
        {
            _repoMock.Setup(r => r.DeactivateAsync(1)).ReturnsAsync(_entity);
            _mapperMock.Setup(m => m.Map<DoctorUpdateDto>(_entity)).Returns(updateDto);

            var result = await _service.DeactivateDoctorAsync(1);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllAsync_Should_ReturnCachedDoctors_WhenCacheExists()
        {
            var cachedDoctors = JsonSerializer.Serialize(dtos);

            _cacheMock
                .Setup(c => c.GetStringAsync(It.IsAny<string>(), default))
                .ReturnsAsync(cachedDoctors);

            var result = await _service.GetAllAsync();

            Assert.Single(result);

            _repoMock.Verify(r => r.GetAllAsync(), Times.Never);
        }
    }
}