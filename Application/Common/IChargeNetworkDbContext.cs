using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IChargeNetworkDbContext
    {
        public DbSet<Group> Groups { get; set; }
        
        public DbSet<ChargeStation> ChargeStations { get; set; }

        public DbSet<Connector> Connectors { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
