﻿using Application.Common;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
    }

    public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IChargeNetworkDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGroupsQueryHandler(IChargeNetworkDbContext dbContext, IMapper mapper)
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
                result = await projection.SingleOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // ignore for now
            }


            return result;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {

            var result = await _mapper.ProjectTo<GroupDto>(_dbContext.Group)
                                        .ToListAsync(cancellationToken);

            return result;
        }
    }
}
