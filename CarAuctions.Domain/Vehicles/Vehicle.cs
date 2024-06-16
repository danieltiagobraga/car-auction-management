namespace CarAuctions.Domain.Vehicles;

public class Vehicle : IVehicle
{
    public string Id { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public ushort Year { get; set; }

    public Vehicle(string id, string manufacturer, string model, ushort year)
    {
        Id = id;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
    }
}