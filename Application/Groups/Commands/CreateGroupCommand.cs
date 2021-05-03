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
        public CreateGroupCommandHandler()
        {

        }

        public Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
