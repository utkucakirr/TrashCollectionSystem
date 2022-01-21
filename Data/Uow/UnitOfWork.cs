using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repositories.ContainerRepo;
using Data.Repositories.VehicleRepo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.Uow
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly VehicleDbContext _context;

        public IVehicleRepository VehicleRepository { get; private set; }
        public IContainerRepository ContainerRepository { get; private set; }

        public UnitOfWork(VehicleDbContext context, ILoggerFactory logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger.CreateLogger("patika1");

            VehicleRepository = new VehicleRepository(_context, _logger);
            ContainerRepository = new ContainerRepository(_context, _logger);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
