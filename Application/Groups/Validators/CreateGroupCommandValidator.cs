using Application.Groups.Commands;
using FluentValidation;

namespace Application.Groups.Validators
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CapacityInAmps).GreaterThan(0);
        }
    }
}
