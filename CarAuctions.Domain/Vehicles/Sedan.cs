namespace CarAuctions.Domain.Vehicles;

public sealed class Sedan : PassengerVehicle
{
    public byte NumberOfDoors { get; set; }

    public Sedan(string id, string manufacturer, string model, ushort year, byte numberOfDoors)
        : base(id, manufacturer, model, year) => NumberOfDoors = numberOfDoors;
}
