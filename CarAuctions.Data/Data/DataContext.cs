using CarAuctions.Domain;
using CarAuctions.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctions.Infra.Data;

public class DataContext : DbContext
{
    public virtual DbSet<Auction> Auctions { get; set; }
    public virtual DbSet<Vehicle> Vehicles { get; set; }
    public virtual DbSet<Bid> Bids { get; set; }


    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        SeedData();
    }

    public void SeedData()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("CarAuctionsDatabase");
    }
}
