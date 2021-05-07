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

namespace Application.ChargeStations.Commands
{
    public class AddChargeStationCommand : IRequest<ChargeStationAdditionResponseModel>
    {
        public Guid GroupId { get; set; }

        public string Name { get; set; }

        public IEnumerable<AddConnectorModel> Connectors { get; }

        public AddChargeStationCommand(Guid groupId, string name, IEnumerable<AddConnectorModel> connectors)
        {
            GroupId = groupId;
            Name = name;
            Connectors = connectors;
        }
    }

    public class AddChargeStationCommandHandler : IRequestHandler<AddChargeStationCommand, ChargeStationAdditionResponseModel>
    {
        private readonly IChargeNetworkDbContext _dbContext;

        public AddChargeStationCommandHandler(IChargeNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChargeStationAdditionResponseModel> Handle(AddChargeStationCommand request, CancellationToken cancellationToken)
        {
            Group group = await GetGroup(request.GroupId);
            ChargeStation chargeStation = CreateChargeStation(request);

            if (group.IsGroupCapacityEnoughForAdditionalChargeStation(chargeStation) == false)
            {
                var suggestionResult = GetSuggestionResult(group, chargeStation);
                return suggestionResult;
            }

            await AddChargeStation(group, chargeStation);

            var succesfulResult = GetSuccesfulResult(chargeStation);
            return succesfulResult;
        }

        private async Task AddChargeStation(Group group, ChargeStation chargeStation)
        {
            group.AddChargeStation(chargeStation);
            _dbContext.ChargeStation.Add(chargeStation);

            _dbContext.Group.Update(group);

            await _dbContext.SaveChangesAsync();
        }

        private async Task<Group> GetGroup(Guid id)
        {
            // poor performance...
            var query = _dbContext.Group.Where(x => x.Id == id)
                                        .Include(x => x.ChargeStations)
                                        .ThenInclude(x => x.Connectors);

            var group = await query.SingleOrDefaultAsync();
            if (group == null)
            {
                throw new NotFoundException(nameof(Group), id);
            }

            return group;
        }

        private ChargeStation CreateChargeStation(AddChargeStationCommand addChargeStationCommand)
        {
            var chargeStation = new ChargeStation
            {
                Name = addChargeStationCommand.Name,
                Connectors = addChargeStationCommand.Connectors.Select(x => new Connector { MaxCurrentInAmps = x.MaxCurrentInAmps }).ToList()
            };

            return chargeStation;
        }
        
        private ChargeStationAdditionResponseModel GetSuggestionResult(Group group, ChargeStation chargeStationToAdd)
        {
            int maxCurrentInAmps = chargeStationToAdd.Connectors.Sum(x => x.MaxCurrentInAmps);

            IEnumerable<ConnectorsCombinationModel> connectors = group.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(maxCurrentInAmps);
            
            var result = new ChargeStationAdditionResponseModel
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

        private ChargeStationAdditionResponseModel GetSuccesfulResult(ChargeStation chargeStation)
        {
            return new ChargeStationAdditionResponseModel
            {
                ChargeStationId = chargeStation.Id
            };
        }
    }
}
