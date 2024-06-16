using CarAuctions.Data.Repositories;
using CarAuctions.Infra.Data;
using CarAuctions.Infra.Repositories;
namespace CarAuctions.Infra.Transactions;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    #region "Here we need to put all repository interfaces of our application"
    public IAuctionRepository AuctionRepository { get; }
    public IVehicleRepository VehicleRepository { get; }
    public IBidRepository BidRepository { get; }
    #endregion

    public UnitOfWork(DataContext context)
    {
        _context = context;

        AuctionRepository = new AuctionRepository(context);
        VehicleRepository = new VehicleRepository(context);
        BidRepository = new BidRepository(context);
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
