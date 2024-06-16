using CarAuctions.Domain;
using CarAuctions.Domain.Vehicles;

namespace CarAuctions.Services;

public interface IAuctionManagerService
{
    void AddVehicle(Vehicle vehicle);
    IEnumerable<IVehicle> SearchVehicles(string type, string manufacturer, string model, int? year);
    string StartAuction(string vehicleId, decimal startingBid);
    void CloseAuction(string vehicleId);
    void PlaceBid(string vehicleId, Bid bid);
}