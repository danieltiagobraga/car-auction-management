namespace CarAuctions.Domain.Vehicles;

public abstract class PassengerVehicle : Vehicle
{
    protected PassengerVehicle(string id, string manufacturer, string model, ushort year)
        : base(id, manufacturer, model, year)
    {
    }
}