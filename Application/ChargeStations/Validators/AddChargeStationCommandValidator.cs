using Application.ChargeStations.Commands;
using Application.Connectors.Validators;
using FluentValidation;

namespace Application.ChargeStations.Validators
{
    public class AddChargeStationCommandValidator : AbstractValidator<AddChargeStationCommand>
    {
        public AddChargeStationCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Connectors).NotNull();
            RuleForEach(x => x.Connectors).SetValidator(new AddConnectorModelValidator());
        }
    }
}
