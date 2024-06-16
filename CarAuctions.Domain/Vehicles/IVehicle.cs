namespace CarAuctions.Domain.Vehicles;

public interface IVehicle
{
    string Id { get; set; }
    string Manufacturer { get; set; }
    string Model { get; set; }
    ushort Year { get; set; }
}