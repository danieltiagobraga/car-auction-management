namespace CarAuctions.Domain.Vehicles;

public sealed class Suv : PassengerVehicle
{
    public byte NumberOfSeats { get; set; }

    public Suv(string id, string manufacturer, string model, ushort year, byte numberOfSeats)
        : base(id, manufacturer, model, year) => NumberOfSeats = numberOfSeats;
}
