using Application.Common;
using Application.Models;
using Domain.Entities;
using Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Connectors.Commands
{
    public class AddConnectorCommand : IRequest<ConnectorAdditionResponseModel>
    {
        public Guid ChargeStationId { get; set; }

        public int MaxCurrentInAmps { get; set; }

        public AddConnectorCommand(Guid chargeStationId, int maxCurrentInAmps)
        {
            ChargeStationId = chargeStationId;
            MaxCurrentInAmps = maxCurrentInAmps;
        }
    }

    public class AddConnectorCommandHandler : IRequestHandler<AddConnectorCommand, ConnectorAdditionResponseModel>
    {
        private readonly IChargeNetworkDbContext _dbContext;

        public AddConnectorCommandHandler(IChargeNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ConnectorAdditionResponseModel> Handle(AddConnectorCommand request, CancellationToken cancellationToken)
        {
            ChargeStation chargeStation = await GetChargeStation(request.ChargeStationId);
            Group group = await GetGroup(chargeStation.GroupId);

            Connector connector = AddToChargeStation(chargeStation, request);

            if (group.IsGroupCapacityEnoughForAdditionalChargeStation(chargeStation) == false)
            {
                var suggestionResult = GetSuggestionResult(group, chargeStation);
                return suggestionResult;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return GetSuccesfulResult(connector);
        }

        private Task<Group> GetGroup(Guid groupId)
        {
            var query = _dbContext.Group.Where(x => x.Id == groupId)
                                        .Include(x => x.ChargeStations)
                                        .ThenInclude(x => x.Connectors)
                                        .AsNoTracking();

            var result = query.SingleOrDefaultAsync();
            return result;
        }

        private Connector AddToChargeStation(ChargeStation chargeStation, AddConnectorCommand request)
        {
            var connector = new Connector
            {
                MaxCurrentInAmps = request.MaxCurrentInAmps
            };

            chargeStation.AddConnector(connector);

            _dbContext.Connector.Add(connector);

            return connector;
        }

        private Task<ChargeStation> GetChargeStation(Guid chargeStationId)
        {
            var chargeStation = _dbContext.ChargeStation.SingleOrDefaultAsync(x => x.Id == chargeStationId);

            if (chargeStation == null)
            {
                throw new NotFoundException(nameof(chargeStation), chargeStationId);
            }

            return chargeStation;
        }

        private ConnectorAdditionResponseModel GetSuggestionResult(Group group, ChargeStation chargeStationToAdd)
        {
            int maxCurrentInAmps = chargeStationToAdd.Connectors.Sum(x => x.MaxCurrentInAmps);

            IEnumerable<ConnectorsCombinationModel> connectors = group.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(maxCurrentInAmps);
            
            var result = new ConnectorAdditionResponseModel
            {
                Suggestions = connectors?.Select(x => new SuggestionsOnAdditionFailureModel
                {
                    ConnectorsToRemoveFromTheGroup = x.Connectors.Select(y => new Models.ConnectorModel
                    {
                        ChargeStationId = y.ChargeStationId,
                        ConnectorId = y.ConnectorId,
                        MaxCurrentInAmps = y.MaxCurrentInAmps,
                    }),
                    NumberOfConnectors = x.NumberOfConnectors,
                    SumOfCurrentInAmps = x.MaxCurrentInAmps
                })
            };

            return result;
        }

        private ConnectorAdditionResponseModel GetSuccesfulResult(Connector connector)
        {
            return new ConnectorAdditionResponseModel
            {
                ConnectorId = connector.Id
            };
        }
    }
}
