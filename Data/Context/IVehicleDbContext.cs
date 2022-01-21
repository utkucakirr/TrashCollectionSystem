using Data.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public interface IVehicleDbContext
    {
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Container> Container { get; set; }

        int SaveChanges();
    }
}
