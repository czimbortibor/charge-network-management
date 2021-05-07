using Application.ChargeStations.Commands;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("groups/{groupId}/charge-stations")]
    public class ChargeStationController : ApiController
    {
        [HttpPost, Route("")]
        public async Task<ActionResult<ChargeStationAdditionResponseModel>> AddChargeStation([FromRoute] Guid groupId, [FromBody] AddChargeStationModel addModel)
        {
            var result = await Mediator.Send(new AddChargeStationCommand(groupId, addModel.Name, addModel.Connectors));

            // TODO should have a different status code when the request couldnt be completed
            return Ok(result);
        }
    }
}
