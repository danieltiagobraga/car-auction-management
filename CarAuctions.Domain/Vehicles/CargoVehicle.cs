namespace CarAuctions.Domain.Vehicles;

public abstract class CargoVehicle : Vehicle
{
    public decimal LoadCapacity { get; set; }

    protected CargoVehicle(string id,
                           string manufacturer,
                           string model,
                           ushort year,
                           decimal loadCapacity)
        : base(id, manufacturer, model, year)
    {
        LoadCapacity = loadCapacity;
    }
}