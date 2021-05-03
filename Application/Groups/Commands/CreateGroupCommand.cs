using Gremlin.Net.Driver;
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
        private readonly GremlinClient _gremlinClient;

        public CreateGroupCommandHandler(GremlinClient gremlinClient)
        {
            _gremlinClient = gremlinClient;
        }

        public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _gremlinClient.SubmitAsync<object>("g.V().count()");
            }
            catch (Exception ex)
            {

            }

            throw new NotImplementedException();
        }
    }
}
