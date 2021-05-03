﻿using Application.Groups.Commands;
using Application.Groups.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("groups")]
    public class GroupController : ApiController
    {
        [HttpPost, Route("")]
        public async Task<ActionResult<Guid>> CreateGroup(CreateGroupCommand request)
        {
            var result = await Mediator.Send(request);

            return CreatedAtRoute("GetGroup", new { Id = result }, result);
        }

        [HttpGet, Route("{id}", Name = "GetGroup")]
        public async Task<ActionResult<GetGroupQuery>> GetGroup(Guid id)
        {
            var result = await Mediator.Send(new GetGroupQuery(id));

            return Ok(result);
        }
    }
}
