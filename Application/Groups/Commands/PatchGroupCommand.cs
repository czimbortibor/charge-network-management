using Application.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class PatchGroupCommand : IRequest
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int CapacityInAmps { get; set; }

        public PatchGroupCommand(Guid id, string name, int capacityInAmps)
        {
            Id = id;
            Name = name;
            CapacityInAmps = capacityInAmps;
        }
    }

    public class PatchGroupCommandHandler : IRequestHandler<PatchGroupCommand>
    {
        private readonly IChargeNetworkDbContext _dbContext;

        public PatchGroupCommandHandler(IChargeNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(PatchGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.Group.SingleOrDefaultAsync(x => x.Id == request.Id);
            if (group == null)
            {
                throw new NotFoundException(nameof(Group), request.Id);
            }

            group.UpdateCapacityInAmps(request.CapacityInAmps);
            group.Name = request.Name;

            _dbContext.Group.Update(group);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
