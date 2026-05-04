
using Application.Dtos;
using Application.Services;
using DeviceManagement.Application.Tests.Builders;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Enums;
using FluentAssertions;
using NSubstitute;

namespace DeviceManagement.Application.Tests.Services;

[TestClass]
public class DeviceServiceTests
{
    private DeviceService _sut = null!;
    private IDeviceRepository _deviceRepo = null!;
    private IUserRepository _userRepo = null!;

    [TestInitialize]
    public void Setup()
    {
        _deviceRepo = Substitute.For<IDeviceRepository>();
        _userRepo = Substitute.For<IUserRepository>();
        _sut = new DeviceService(_deviceRepo, _userRepo);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsAllDevicesMappedToDtos()
    {
        var devices = new List<Device>
        {
            DeviceBuilder.Default().WithId(1).WithName("iPhone 15").Build(),
            DeviceBuilder.Default().WithId(2).WithName("Galaxy S24").Build()
        };
        _deviceRepo.GetAllWithUsersAsync().Returns(devices);

        var result = await _sut.GetAllAsync();

        var list = result.ToList();
        list.Should().HaveCount(2);
        list[0].Name.Should().Be("iPhone 15");
        list[1].Name.Should().Be("Galaxy S24");
    }

    [TestMethod]
    public async Task GetAllAsync_WhenNoDevices_ReturnsEmptyList()
    {
        _deviceRepo.GetAllWithUsersAsync().Returns(new List<Device>());

        var result = await _sut.GetAllAsync();

        result.Should().BeEmpty();
    }


    [TestMethod]
    public async Task GetByIdAsync_ExistingDevice_ReturnsMappedDto()
    {
        var device = DeviceBuilder.Default()
            .WithId(1).WithName("Pixel 8").WithManufacturer("Google").WithRam(12)
            .Build();
        _deviceRepo.GetByIdWithUserAsync(1).Returns(device);

        var result = await _sut.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Pixel 8");
        result.Manufacturer.Should().Be("Google");
        result.RamAmount.Should().Be(12);
    }

    [TestMethod]
    public async Task GetByIdAsync_NonExistingDevice_ReturnsNull()
    {
        _deviceRepo.GetByIdWithUserAsync(99).Returns((Device?)null);

        var result = await _sut.GetByIdAsync(99);

        result.Should().BeNull();
    }


    [TestMethod]
    public async Task CreateAsync_ValidDevice_CreatesAndReturnsMappedDto()
    {
        var dto = new CreateDeviceDto(
            "iPhone 15 Pro", "Apple", DeviceType.Phone,
            "iOS", "17.2", "A17 Pro", 8, "Flagship", null);

        var savedDevice = DeviceBuilder.Default().WithId(10).WithName("iPhone 15 Pro").Build();
        _deviceRepo.CreateAsync(Arg.Any<Device>()).Returns(savedDevice);
        _deviceRepo.GetByIdWithUserAsync(10).Returns(savedDevice);

        var result = await _sut.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Name.Should().Be("iPhone 15 Pro");
        await _deviceRepo.Received(1).CreateAsync(Arg.Any<Device>());
    }

    [TestMethod]
    public async Task CreateAsync_WithNonExistingAssignedUser_ThrowsKeyNotFoundException()
    {
        var dto = new CreateDeviceDto(
            "Test", "TestCo", DeviceType.Phone,
            "Android", "14", "CPU", 8, null, 999);

        _userRepo.GetByIdAsync(999).Returns((User?)null);

        // MSTest way to assert exceptions on async methods
        await FluentActions
            .Awaiting(() => _sut.CreateAsync(dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }

    [TestMethod]
    public async Task CreateAsync_WithAssignedUserId_VerifiesUserExists()
    {
        var user = UserBuilder.Default().WithId(5).Build();
        var dto = new CreateDeviceDto(
            "Test", "TestCo", DeviceType.Phone,
            "Android", "14", "CPU", 8, null, 5);

        _userRepo.GetByIdAsync(5).Returns(user);

        var savedDevice = DeviceBuilder.Default().WithId(1).WithAssignedUser(5).Build();
        savedDevice.AssignedUser = user;
        _deviceRepo.CreateAsync(Arg.Any<Device>()).Returns(savedDevice);
        _deviceRepo.GetByIdWithUserAsync(1).Returns(savedDevice);

        await _sut.CreateAsync(dto);

        await _userRepo.Received(1).GetByIdAsync(5);
    }


    [TestMethod]
    public async Task UpdateAsync_ExistingDevice_UpdatesAndReturnsDto()
    {
        var existing = DeviceBuilder.Default().WithId(1).WithName("Old Name").Build();
        var updated = DeviceBuilder.Default().WithId(1).WithName("New Name").Build();

        _deviceRepo.GetByIdAsync(1).Returns(existing);
        _deviceRepo.UpdateAsync(Arg.Any<Device>()).Returns(existing);
        _deviceRepo.GetByIdWithUserAsync(1).Returns(updated);

        var dto = new UpdateDeviceDto(
            "New Name", "TestCo", DeviceType.Phone,
            "Android", "14", "CPU", 8, null, null);

        var result = await _sut.UpdateAsync(1, dto);

        result.Name.Should().Be("New Name");
        await _deviceRepo.Received(1).UpdateAsync(Arg.Any<Device>());
    }

    [TestMethod]
    public async Task UpdateAsync_NonExistingDevice_ThrowsKeyNotFoundException()
    {
        _deviceRepo.GetByIdAsync(99).Returns((Device?)null);

        var dto = new UpdateDeviceDto(
            "X", "Y", DeviceType.Phone, "Z", "1", "CPU", 4, null, null);

        await FluentActions
            .Awaiting(() => _sut.UpdateAsync(99, dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*99*");
    }


    [TestMethod]
    public async Task DeleteAsync_ExistingDevice_CallsRepositoryDelete()
    {
        var device = DeviceBuilder.Default().WithId(1).Build();
        _deviceRepo.GetByIdAsync(1).Returns(device);

        await _sut.DeleteAsync(1);

        await _deviceRepo.Received(1).DeleteAsync(device);
    }

    [TestMethod]
    public async Task DeleteAsync_NonExistingDevice_ThrowsKeyNotFoundException()
    {
        _deviceRepo.GetByIdAsync(99).Returns((Device?)null);

        await FluentActions
            .Awaiting(() => _sut.DeleteAsync(99))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task SelfAssignAsync_UnassignedDevice_AssignsSuccessfully()
    {
        var device = DeviceBuilder.Default().WithId(1).Unassigned().Build();
        var updated = DeviceBuilder.Default().WithId(1).WithAssignedUser(5).Build();

        _deviceRepo.GetByIdAsync(1).Returns(device);
        _deviceRepo.GetByIdWithUserAsync(1).Returns(updated);

        var result = await _sut.SelfAssignAsync(1, 5);

        result.AssignedUserId.Should().Be(5);
        await _deviceRepo.Received(1).AssignUserAsync(1, 5);
    }

    [TestMethod]
    public async Task SelfAssignAsync_AlreadyAssignedDevice_ThrowsInvalidOperationException()
    {
        var device = DeviceBuilder.Default().WithId(1).WithAssignedUser(3).Build();
        _deviceRepo.GetByIdAsync(1).Returns(device);

        await FluentActions
            .Awaiting(() => _sut.SelfAssignAsync(1, 5))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already assigned*");
    }

    [TestMethod]
    public async Task SelfUnassignAsync_WhenUserOwnsDevice_UnassignsSuccessfully()
    {
        var device = DeviceBuilder.Default().WithId(1).WithAssignedUser(5).Build();
        var updated = DeviceBuilder.Default().WithId(1).Unassigned().Build();

        _deviceRepo.GetByIdAsync(1).Returns(device);
        _deviceRepo.GetByIdWithUserAsync(1).Returns(updated);

        var result = await _sut.SelfUnassignAsync(1, 5);

        result.AssignedUserId.Should().BeNull();
        await _deviceRepo.Received(1).AssignUserAsync(1, null);
    }

    [TestMethod]
    public async Task SelfUnassignAsync_WhenUserDoesNotOwnDevice_ThrowsUnauthorizedAccessException()
    {
        var device = DeviceBuilder.Default().WithId(1).WithAssignedUser(3).Build();
        _deviceRepo.GetByIdAsync(1).Returns(device);

        await FluentActions
            .Awaiting(() => _sut.SelfUnassignAsync(1, 5))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*only unassign*");
    }


    [TestMethod]
    public async Task GetByIdAsync_DeviceWithAssignedUser_MapsUserNameCorrectly()
    {
        var user = UserBuilder.Default().WithId(3).WithName("Alice Johnson").Build();
        var device = DeviceBuilder.Default().WithId(1).WithAssignedUser(3).Build();
        device.AssignedUser = user;

        _deviceRepo.GetByIdWithUserAsync(1).Returns(device);

        var result = await _sut.GetByIdAsync(1);

        result!.AssignedUserId.Should().Be(3);
        result.AssignedUserName.Should().Be("Alice Johnson");
    }

    [TestMethod]
    public async Task GetByIdAsync_PhoneType_MapsTypeLabelCorrectly()
    {
        var device = DeviceBuilder.Default().WithId(1).WithType(DeviceType.Phone).Build();
        _deviceRepo.GetByIdWithUserAsync(1).Returns(device);

        var result = await _sut.GetByIdAsync(1);

        result?.TypeLabel.Should().Be("Phone");
    }
}
