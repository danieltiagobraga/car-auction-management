namespace CarAuctions.Domain.Vehicles;

public sealed class Hatchback : PassengerVehicle
{
    public byte NumberOfDoors { get; set; }

    public Hatchback(string id, string manufacturer, string model, ushort year, byte numberOfDoors)
        : base(id, manufacturer, model, year) => NumberOfDoors = numberOfDoors;
}
