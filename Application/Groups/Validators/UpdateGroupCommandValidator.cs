using Application.Groups.Commands;
using FluentValidation;

namespace Application.Groups.Validators
{
    public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CapacityInAmps).GreaterThan(0);
        }
    }
}
