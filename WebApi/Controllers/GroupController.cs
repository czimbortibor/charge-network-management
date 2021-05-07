using Application.Groups.Commands;
using Application.Groups.Queries;
using Application.Models;
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

        [HttpGet, Route("dump-db")]
        public async Task<ActionResult<GetGroupQuery>> GetGroups()
        {
            var result = await Mediator.Send(new GetGroupsQuery());

            return Ok(result);
        }

        [HttpPatch, Route("{id}")]
        public async Task<ActionResult<Guid>> PatchGroup([FromRoute] Guid id, [FromBody] PatchGroupModel patchModel)
        {
            await Mediator.Send(new PatchGroupCommand(id, patchModel.Name, patchModel.CapacityInAmps));

            return NoContent();
        }
    }
}
