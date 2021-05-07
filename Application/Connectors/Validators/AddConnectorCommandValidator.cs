using Application.Models;
using FluentValidation;

namespace Application.Connectors.Validators
{
    public class AddConnectorModelValidator : AbstractValidator<AddConnectorModel>
    {
        public AddConnectorModelValidator()
        {
            RuleFor(x => x.MaxCurrentInAmps).GreaterThan(0);
        }
    }

    public class AddConnectorCommandValidator : AbstractValidator<AddConnectorModel>
    {
        public AddConnectorCommandValidator()
        {
            RuleFor(x => x.MaxCurrentInAmps).GreaterThan(0);
        }
    }
}
