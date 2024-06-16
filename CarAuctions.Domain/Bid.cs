namespace CarAuctions.Domain;

public class Bid
{
    public string Id { get; set; }
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public string AuctionId { get; set; }
    public string VehicleId { get; set; }

    public Bid(string id, decimal amount, string userId, string auctionId, string vehicleId)
    {
        Id = id;
        Amount = amount;
        UserId = userId;
        VehicleId = vehicleId;
        AuctionId = auctionId;
    }
}
