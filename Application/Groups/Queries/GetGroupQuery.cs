using Application.Groups.Models;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetGroupQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<GroupDto> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
