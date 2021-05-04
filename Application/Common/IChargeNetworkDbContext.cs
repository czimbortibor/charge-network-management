using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IChargeNetworkDbContext
    {
        public DbSet<Group> Group { get; set; }
        
        public DbSet<ChargeStation> ChargeStation { get; set; }

        public DbSet<Connector> Connector { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
