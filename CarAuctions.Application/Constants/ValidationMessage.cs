using System.Globalization;
using System.Resources;

namespace CarAuctions.Services.Constants;

public static class ValidationMessage
{
    private static readonly ResourceManager ResourceManager = new("CarAuctions.Services.Resources.ValidationMessage", typeof(ValidationMessage).Assembly);

    public static string? VehicleAlreadyExists => ResourceManager.GetString(nameof(VehicleAlreadyExists), CultureInfo.CurrentCulture);
    public static string? VehicleDoesNotExist => ResourceManager.GetString(nameof(VehicleDoesNotExist), CultureInfo.CurrentCulture);
    public static string? AuctionAlreadyActive => ResourceManager.GetString(nameof(AuctionAlreadyActive), CultureInfo.CurrentCulture);
    public static string? AuctionIsNotActive => ResourceManager.GetString(nameof(AuctionIsNotActive), CultureInfo.CurrentCulture);
    public static string? AuctionDoesNotExist => ResourceManager.GetString(nameof(AuctionDoesNotExist), CultureInfo.CurrentCulture);
    public static string? BidAmountMustBeHigherThanCurrentBid => ResourceManager.GetString(nameof(BidAmountMustBeHigherThanCurrentBid), CultureInfo.CurrentCulture);
    public static string? InvalidVehicleType => ResourceManager.GetString(nameof(InvalidVehicleType), CultureInfo.CurrentCulture);
    public static string? ManufacturerAndModelCannotBeEmpty => ResourceManager.GetString(nameof(ManufacturerAndModelCannotBeEmpty), CultureInfo.CurrentCulture);
    public static string? YearIsOutOfRange => ResourceManager.GetString(nameof(YearIsOutOfRange), CultureInfo.CurrentCulture);
    public static string? ParameterVehicle => ResourceManager.GetString(nameof(ParameterVehicle), CultureInfo.CurrentCulture);
}
