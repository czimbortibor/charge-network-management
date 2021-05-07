using Application.Connectors.Commands;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("groups/charge-stations/{chargeStationId}/connectors")]
    public class ConnectorController : ApiController
    {
        [HttpPost, Route("")]
        public async Task<ActionResult<ConnectorAdditionResponseModel>> AddConnector([FromRoute] Guid chargeStationId, [FromBody] AddConnectorModel addModel)
        {
            var result = await Mediator.Send(new AddConnectorCommand(chargeStationId, addModel.MaxCurrentInAmps));

            // TODO not OK (everytime)
            return Ok(result);
        }
    }
}
