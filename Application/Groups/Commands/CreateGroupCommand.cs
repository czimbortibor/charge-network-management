using Application.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        
        public int CapacityInAmps { get; set; }
    }

    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Guid>
    {
        private readonly IChargeNetworkDbContext _dbContext;

        public CreateGroupCommandHandler(IChargeNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = new Group(request.Name, request.CapacityInAmps);

            _dbContext.Group.Add(group);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return group.Id;
        }
    }
}
