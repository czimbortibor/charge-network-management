using Application.Groups.Commands;
using FluentValidation;

namespace Application.Groups.Validators
{
    public class PatchGroupCommandValidator : AbstractValidator<PatchGroupCommand>
    {
        public PatchGroupCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CapacityInAmps).GreaterThan(0);
        }
    }
}
