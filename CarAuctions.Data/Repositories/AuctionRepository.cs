using CarAuctions.Data.Repositories.Base;
using CarAuctions.Domain;
using CarAuctions.Infra.Data;

namespace CarAuctions.Data.Repositories;

public class AuctionRepository(DataContext context) : Repository<Auction>(context), IAuctionRepository
{
}
