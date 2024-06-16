using CarAuctions.Data.Repositories.Base;
using CarAuctions.Domain.Vehicles;
using CarAuctions.Infra.Data;
using CarAuctions.Infra.Repositories;

namespace CarAuctions.Data.Repositories;

public class VehicleRepository(DataContext context) : Repository<Vehicle>(context), IVehicleRepository 
{
}
