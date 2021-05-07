using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChargeStations.Commands
{
    public class DeleteChargeStationCommand : IRequest<Unit>
    {
        public Guid ChargeStationId { get; set; }

        public DeleteChargeStationCommand(Guid chargeStationId)
        {
            ChargeStationId = chargeStationId;
        }
    }

    public class DeleteChargeStationCommandHandler : IRequestHandler<DeleteChargeStationCommand>
    {
        private readonly IChargeNetworkDbContext _dbContext;

        public DeleteChargeStationCommandHandler(IChargeNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteChargeStationCommand request, CancellationToken cancellationToken)
        {
            var query = _dbContext.ChargeStation.Where(x => x.Id == request.ChargeStationId)
                                                .Include(x => x.Connectors);

            var chargeStation = await query.SingleOrDefaultAsync(cancellationToken);

            if (chargeStation == null)
            {
                return await Unit.Task;
            }

            _dbContext.ChargeStation.Remove(chargeStation);

            if (chargeStation.Connectors != null)
            {
                _dbContext.Connector.RemoveRange(chargeStation.Connectors);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
