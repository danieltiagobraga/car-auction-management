using CarAuctions.Domain;
using CarAuctions.Domain.Vehicles;
using CarAuctions.Infra.Transactions;
using CarAuctions.Services.Constants;

namespace CarAuctions.Services;

/// <summary>
/// This service is responsable for orchestrate the different vehicles auctions
/// </summary>
public class AuctionManagerService : IAuctionManagerService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuctionManagerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Methods
    public void AddVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            throw new ArgumentNullException(nameof(vehicle), ValidationMessage.InvalidVehicleType);
        }

        if (string.IsNullOrWhiteSpace(vehicle.Manufacturer) || string.IsNullOrWhiteSpace(vehicle.Model))
        {
            throw new ArgumentException(ValidationMessage.ManufacturerAndModelCannotBeEmpty);
        }

        if (vehicle.Year < 1886 || vehicle.Year > DateTime.Now.Year) // Assuming no cars before 1886
        {
            throw new ArgumentOutOfRangeException(ValidationMessage.YearIsOutOfRange, nameof(vehicle.Year));
        }

        bool exists = _unitOfWork.VehicleRepository.Get(vehicle.Id) is not null;
        if (exists)
        {
            throw new ArgumentException(ValidationMessage.VehicleAlreadyExists);
        }

        _unitOfWork.VehicleRepository.Add(vehicle);
        _unitOfWork.Commit();
    }

    public IEnumerable<IVehicle> SearchVehicles(string? type, string? manufacturer, string? model, int? year) =>
        _unitOfWork.VehicleRepository.GetAll()
            .Where(vehicle =>
                    (type == null || vehicle.GetType().Name == type) &&
                    (manufacturer == null || vehicle.Manufacturer == manufacturer) &&
                    (model == null || vehicle.Model == model) &&
                    (year == null || vehicle.Year == year)
            );

    public string StartAuction(string vehicleId, decimal startingBid)
    {
        IVehicle vehicle = _unitOfWork.VehicleRepository.Get(vehicleId) ?? throw new ArgumentException(ValidationMessage.VehicleDoesNotExist);

        var auction = _unitOfWork.AuctionRepository.Get(vehicleId);
        if (auction is not null && auction.Active)
        {
            throw new InvalidOperationException(ValidationMessage.AuctionAlreadyActive);
        }

        auction = new(vehicleId, vehicle, startingBid, active: true);

        _unitOfWork.AuctionRepository.Add(auction);

        return vehicleId;
    }
    
    public void CloseAuction(string vehicleId)
    {
        Auction auction = _unitOfWork.AuctionRepository.Get(vehicleId) ?? throw new ArgumentException(ValidationMessage.AuctionDoesNotExist);
        if (!auction.Active)
        {
            throw new InvalidOperationException(ValidationMessage.AuctionIsNotActive);
        }

        auction.Active = false;
    }

    public void PlaceBid(string vehicleId, Bid bid)
    {
        Auction auction = _unitOfWork.AuctionRepository.Get(vehicleId) ?? throw new ArgumentException(ValidationMessage.AuctionDoesNotExist);
        if (!auction.Active)
        {
            throw new InvalidOperationException(ValidationMessage.AuctionIsNotActive);
        }

        if (bid.Amount <= auction.CurrentBid)
        {
            throw new ArgumentException(ValidationMessage.BidAmountMustBeHigherThanCurrentBid);
        }

        auction.SetCurrentBid(bid.Amount);
        auction.AddBid(bid);

        _unitOfWork.BidRepository.Add(bid);
        _unitOfWork.AuctionRepository.Update(auction);
    }
}