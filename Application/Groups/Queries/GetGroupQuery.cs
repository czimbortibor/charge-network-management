using Application.Common;
using Application.Groups.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
        private readonly IChargeNetworkDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGroupQueryHandler(IChargeNetworkDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Group.Where(x => x.Id == request.Id);

            var projection = _mapper.ProjectTo<GroupDto>(query);

            GroupDto result = null;
            try
            {
                result = await projection.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // ignore for now
            }


            return result;
        }
    }
}
