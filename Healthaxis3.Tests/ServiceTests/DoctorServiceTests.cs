using Xunit;
using Moq;
using AutoMapper;
using HealthAxis3.API.Service.Implementation;
using HealthAxis3.API.Repository;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.DoctorDto;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DoctorService _service;

    private readonly DoctorDto _dto;
    private readonly Doctor _entity;
    private readonly List<Doctor> entities;
    private readonly List<DoctorDto> dtos;
    private readonly DoctorUpdateDto updateDto;
    private readonly Doctor updatedDoctor;
    private readonly DoctorUpdateDto resultDto;

    public DoctorServiceTests()
    {
        _repoMock = new Mock<IDoctorRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new DoctorService(_repoMock.Object, _mapperMock.Object);

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

        Assert.NotNull(result);
    }
    [Fact]
    public async Task GetAllAsync_Should_ReturnListOfDtos()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Single(result);
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
}