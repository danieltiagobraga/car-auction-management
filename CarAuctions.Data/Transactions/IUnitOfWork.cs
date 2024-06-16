using CarAuctions.Data.Repositories;
using CarAuctions.Infra.Repositories;

namespace CarAuctions.Infra.Transactions;

public interface IUnitOfWork : IDisposable
{
    #region "Here we need to put all repository interfaces of our application"
    IAuctionRepository AuctionRepository { get; }
    IVehicleRepository VehicleRepository { get; }
    IBidRepository BidRepository { get; }
    #endregion

    void Commit();
}
