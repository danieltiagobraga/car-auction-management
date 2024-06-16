using CarAuctions.Domain.Vehicles;
using CarAuctions.Services.Constants;

namespace CarAuctions.Services;

public static class VehicleFactory
{
    public static CargoVehicle CreateCargoVehicle(string type, string id, string manufacturer, string model, ushort year, int loadCapacity) =>
        type switch
        {
            nameof(Truck) => new Truck(id, manufacturer, model, year, loadCapacity),
            _ => throw new ArgumentException(ValidationMessage.InvalidVehicleType)
        };

    public static PassengerVehicle CreatePassengerVehicle(string type, string id, string manufacturer, string model, ushort year, byte additionalAttribute) =>
        type switch
        {
            nameof(Hatchback) => new Hatchback(id, manufacturer, model, year, additionalAttribute),
            nameof(Sedan) => new Sedan(id, manufacturer, model, year, additionalAttribute),
            nameof(Suv) => new Suv(id, manufacturer, model, year, additionalAttribute),
            _ => throw new ArgumentException(ValidationMessage.InvalidVehicleType)
        };
}
