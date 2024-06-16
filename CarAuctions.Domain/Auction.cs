using CarAuctions.Domain.Vehicles;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuctions.Domain;

public class Auction
{
    public string Id { get; set; }
    public bool Active { get; set; }
    public decimal StartingBid { get; private set; }
    public decimal CurrentBid { get; private set; }

    [NotMapped]
    public IVehicle Vehicle { get; set; }

    // Related ?
    [NotMapped]
    public List<Bid> Bids { get; set; }

    public Auction()
    {

    }

    public Auction(string id,
                   IVehicle vehicle,
                   decimal startingBid,
                   bool active)
    {
        Id = id;
        Active = active;
        Vehicle = vehicle;
        StartingBid = startingBid;
        CurrentBid = startingBid;
        Bids = [];
    }

    public void SetCurrentBid(decimal bid)  =>
        CurrentBid = bid;
    public void AddBid(Bid bid) =>
        Bids.Add(bid);
}
