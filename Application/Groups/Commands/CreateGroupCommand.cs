using AutoMapper;
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
        private readonly IMapper _mapper;

        public CreateGroupCommandHandler(GremlinClient gremlinClient, IMapper mapper)
        {
            _gremlinClient = gremlinClient;
            _mapper = mapper;
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
