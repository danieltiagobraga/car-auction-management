using CarAuctions.Domain;
using CarAuctions.Domain.Vehicles;
using CarAuctions.Infra.Transactions;
using CarAuctions.Data.Repositories;
using CarAuctions.Infra.Repositories;
using CarAuctions.Services.Constants;
using Moq;
using System.Globalization;
using System;

namespace CarAuctions.Services.Tests;
public class AuctionManagerServiceTests
{
    private const string AuctionId = "Vehicle1Id";
    private const string VehicleId = "Vehicle1Id";

    private const Auction NullAuction = null;
    private const Vehicle NullVehicle = null;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
    private readonly Mock<IBidRepository> _bidRepositoryMock;
    private readonly AuctionManagerService _auctionManagerService;

    public AuctionManagerServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _auctionRepositoryMock = new Mock<IAuctionRepository>();
        _bidRepositoryMock = new Mock<IBidRepository>();

        _unitOfWorkMock.Setup(u => u.VehicleRepository).Returns(_vehicleRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.AuctionRepository).Returns(_auctionRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.BidRepository).Returns(_bidRepositoryMock.Object);

        _auctionManagerService = new AuctionManagerService(_unitOfWorkMock.Object);
    }

    // Vehicles
    [Fact]
    public void AddVehicle_ThrowsArgumentException_VehicleAlreadyExists()
    {
        // Arrange
        var vehicle = VehicleFactory.CreateCargoVehicle(nameof(Truck), VehicleId, manufacturer: "Volvo", model: "FH", 2024, 4000);

        _vehicleRepositoryMock
            .Setup(v => v.Get(AuctionId))
            .Returns(vehicle);

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _auctionManagerService.AddVehicle(vehicle));

        // Assert
        Assert.Equal(ValidationMessage.VehicleAlreadyExists, ex.Message);
    }

    [Fact]
    public void AddVehicle_ThrowsArgumentNullException_NullVehicle()
    {
        // Arrange

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => _auctionManagerService.AddVehicle(NullVehicle));

        // Assert
        Assert.Equal($"{ValidationMessage.InvalidVehicleType} {ValidationMessage.ParameterVehicle}", exception.Message);
    }

    [Fact]
    public void AddVehicle_ThrowsArgumentException_ManufacturerAndModelCannotBeEmpty()
    {
        // Arrange
        var vehicle = VehicleFactory.CreateCargoVehicle(nameof(Truck), VehicleId, manufacturer: string.Empty, model: string.Empty, 2024, 4000);

        _vehicleRepositoryMock
            .Setup(v => v.Get(AuctionId))
            .Returns(vehicle);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => _auctionManagerService.AddVehicle(vehicle));

        // Assert
        Assert.Equal(ValidationMessage.ManufacturerAndModelCannotBeEmpty, exception.Message);
    }

    [Fact]
    public void AddVehicle_Success()
    {
        // Arrange
        var vehicle = VehicleFactory.CreatePassengerVehicle(nameof(Suv), VehicleId, manufacturer: "Renault", model: "Megane", year: 2024, additionalAttribute: 4);

        _vehicleRepositoryMock
            .Setup(v => v.Get(AuctionId))
            .Returns(NullVehicle);

        // Act
        _auctionManagerService.AddVehicle(vehicle);

        // Assert
        _vehicleRepositoryMock.Verify(v => v.Add(vehicle), Times.Once);
    }

    [Fact]
    public void SearchVehicles_FiltersVehiclesCorrectly_SingleResult()
    {
        // Arrange
        var vehicle1 = VehicleFactory.CreateCargoVehicle(nameof(Truck), VehicleId, manufacturer: "Volvo", model: "FH", 2024, 4000);
        var vehicle2 = VehicleFactory.CreatePassengerVehicle(nameof(Suv), VehicleId, manufacturer: "Renault", model: "Megane", year: 2024, additionalAttribute: 4);

        _vehicleRepositoryMock
            .Setup(v => v.GetAll())
            .Returns([vehicle1, vehicle2]);

        // Act
        var results = _auctionManagerService.SearchVehicles(nameof(Truck), "Volvo", "FH", 2024);

        // Assert
        Assert.Single(results);
        Assert.Contains(vehicle1, results);
    }

    [Fact]
    public void SearchVehicles_FiltersVehiclesCorrectly_EmptyResult()
    {
        // Arrange
        var vehicle1 = VehicleFactory.CreateCargoVehicle(nameof(Truck), VehicleId, manufacturer: "Volvo", model: "FH", 2024, 4000);
        var vehicle2 = VehicleFactory.CreatePassengerVehicle(nameof(Suv), VehicleId, manufacturer: "Renault", model: "Megane", year: 2024, additionalAttribute: 4);

        _vehicleRepositoryMock
            .Setup(v => v.GetAll())
            .Returns([vehicle1, vehicle2]);

        // Act
        var results = _auctionManagerService.SearchVehicles(nameof(Suv), "Ford", "F350", 2010);

        // Assert
        Assert.Empty(results);
    }


    // Auction
    [Fact]
    public void StartAuction_ThrowsArgumentException_VehicleDoesNotExist()
    {
        // Arrange
        _vehicleRepositoryMock
            .Setup(v => v.Get(AuctionId))
            .Returns(NullVehicle);

        
        // Act
        var exception = Assert.Throws<ArgumentException>(() => _auctionManagerService.StartAuction(AuctionId, 1000));

        // Assert
        Assert.Equal(ValidationMessage.VehicleDoesNotExist, exception.Message);
    }

    [Fact]
    public void StartAuction_ThrowsInvalidOperationException_AuctionAlreadyActive()
    {
        // Arrange
        var vehicle = VehicleFactory.CreatePassengerVehicle(nameof(Suv), VehicleId, manufacturer: "Renault", model: "Megane", year: 2024, additionalAttribute: 4);

        _vehicleRepositoryMock
            .Setup(v => v.Get(AuctionId))
            .Returns(vehicle);

        var auction = new Auction(AuctionId, new Mock<IVehicle>().Object, startingBid: 1000, active: true);

        _auctionRepositoryMock.Setup(a => a.Get(AuctionId)).Returns(auction);

        // Act 
        var exception = Assert.Throws<InvalidOperationException>(() => _auctionManagerService.StartAuction(AuctionId, 1000));

        // Assert
        Assert.Equal(ValidationMessage.AuctionAlreadyActive, exception.Message);
    }

    [Fact]
    public void StartAuction_NewAuction_AddsAuction()
    {
        // Arrange
        var vehicle = VehicleFactory.CreatePassengerVehicle(nameof(Suv), VehicleId, manufacturer: "Renault", model: "Megane", year: 2024, additionalAttribute: 4);
        _vehicleRepositoryMock.Setup(v => v.Get(AuctionId)).Returns(vehicle);

        _auctionRepositoryMock.Setup(a => a.Get(AuctionId)).Returns(NullAuction);

        // Act
        var vehicleId = _auctionManagerService.StartAuction(AuctionId, 1000);

        // Assert
        _auctionRepositoryMock.Verify(a => a.Add(It.Is<Auction>(a => a.Id == AuctionId && a.StartingBid == 1000)), Times.Once);
        Assert.Equal(AuctionId, vehicleId);
    }

    [Fact]
    public void CloseAuction_ThrowsArgumentException_AuctionDoesNotExist()
    {
        // Arrange
        _auctionRepositoryMock
            .Setup(a => a.Get(AuctionId))
            .Returns(NullAuction);

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _auctionManagerService.CloseAuction("123"));

        // Assert
        Assert.Equal(ValidationMessage.AuctionDoesNotExist, ex.Message);
    }

    [Fact]
    public void CloseAuction_ThrowsInvalidOperationException_AuctionNotActive()
    {
        // Arrange
        var auction = new Auction(AuctionId, new Mock<IVehicle>().Object, startingBid: 1000, active: false);

        _auctionRepositoryMock
            .Setup(a => a.Get(AuctionId))
            .Returns(auction);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => _auctionManagerService.CloseAuction(auction.Id));

        // Assert
        Assert.Equal(ValidationMessage.AuctionIsNotActive, exception.Message);
    }

    [Fact]
    public void CloseAuction_AuctionIsActive()
    {
        // Arrange
        var auction = new Auction(AuctionId, new Mock<IVehicle>().Object, startingBid: 1000, active: true);

        _auctionRepositoryMock
            .Setup(a => a.Get(AuctionId))
            .Returns(auction);

        // Act
        _auctionManagerService.CloseAuction(AuctionId);

        // Assert
        Assert.False(auction.Active);
    }

    // Bids
    [Fact]
    public void PlaceBid_ThrowsArgumentException_AuctionDoesNotExist()
    {
        // Arrange
        _auctionRepositoryMock.Setup(a => a.Get(AuctionId)).Returns(NullAuction);

        var bid = new Bid("bid1Id", 1000, "user1Id", "auction1Id", AuctionId);

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _auctionManagerService.PlaceBid(AuctionId, bid));

        // Assert
        Assert.Equal(ValidationMessage.AuctionDoesNotExist, ex.Message);
    }

    [Fact]
    public void PlaceBid_ThrowsInvalidOperationException_AuctionNotActive()
    {
        // Arrange
        var auction = new Auction(AuctionId, new Mock<IVehicle>().Object, startingBid: 1000, active: false);

        _auctionRepositoryMock.Setup(a => a.Get(AuctionId)).Returns(auction);

        var bid = new Bid("bid1Id", 1000, "user1Id", "auction1Id", AuctionId);

        // Act 
        var exception = Assert.Throws<InvalidOperationException>(() => _auctionManagerService.PlaceBid(AuctionId, bid));

        // Assert
        Assert.Equal(ValidationMessage.AuctionIsNotActive, exception.Message);
    }

    [Fact]
    public void PlaceBid_ThrowsArgumentException_BidAmountTooLow()
    {
        // Arrange
        var auction = new Auction(AuctionId, new Mock<IVehicle>().Object, startingBid: 1000, active: true); 

        _auctionRepositoryMock
            .Setup(a => a.Get(AuctionId))
            .Returns(auction);

        var bid = new Bid("bid1Id", 1000, "user1Id", "auction1Id", AuctionId);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => _auctionManagerService.PlaceBid(AuctionId, bid));

        // Assert
        Assert.Equal(ValidationMessage.BidAmountMustBeHigherThanCurrentBid, exception.Message);
    }

    [Fact]
    public void PlaceBid_ValidBid()
    {
        // Arrange
        var auction = new Auction(AuctionId, new Mock<IVehicle>().Object, startingBid: 1000, active: true);

        _auctionRepositoryMock
            .Setup(a => a.Get(AuctionId))
            .Returns(auction);

        var bid = new Bid("bid1Id", 1500, "user1Id", "auction1Id", AuctionId);


        // Act
        _auctionManagerService.PlaceBid(AuctionId, bid);

        // Assert
        _bidRepositoryMock.Verify(b => b.Add(bid), Times.Once);
        _auctionRepositoryMock.Verify(a => a.Update(auction), Times.Once);
    }
}
