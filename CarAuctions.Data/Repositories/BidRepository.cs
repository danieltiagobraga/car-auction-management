using CarAuctions.Data.Repositories.Base;
using CarAuctions.Domain;
using CarAuctions.Infra.Data;

namespace CarAuctions.Data.Repositories;

public class BidRepository(DataContext context) : Repository<Bid>(context), IBidRepository
{
}
