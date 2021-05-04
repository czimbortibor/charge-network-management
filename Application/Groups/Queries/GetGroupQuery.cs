using Application.Groups.Models;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries
{
    public class GetGroupQuery : IRequest<GroupDto>
    {
        public GetGroupQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }


    public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, GroupDto>
    {
        private readonly IGremlinClient _gremlinClient;
        private readonly IGremlinQuerySource _gremlinSource;

        public GetGroupQueryHandler(IGremlinClient gremlinClient, IGremlinQuerySource gremlinSource)
        {
            _gremlinClient = gremlinClient;
            _gremlinSource = gremlinSource;
        }

        public async Task<GroupDto> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            var result = await _gremlinSource.V($"{request.Id}");
            //var result = _gremlinClient.SubmitAsync($"g.V('{request.Id}')");
            
            return null;
        }
    }
}
