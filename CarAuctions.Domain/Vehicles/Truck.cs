namespace CarAuctions.Domain.Vehicles;

public sealed class Truck : CargoVehicle
{
    public Truck(string id, string manufacturer, string model, ushort year, decimal loadCapacity)
        : base(id, manufacturer, model, year, loadCapacity)
    {
    }
}