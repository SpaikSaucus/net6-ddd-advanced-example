using AuthorizationOperation.Application.UserCases.Create.Commands;
using FluentValidation;

namespace AuthorizationOperation.Application.UserCases.Create.Validations
{
    public class CreateAuthorizationValidation : AbstractValidator<CreateAuthorizationCommand>
    {
        public CreateAuthorizationValidation()
        {
            this.RuleFor(command => command.UUID)
                .NotEmpty()
                    .WithMessage("The UUID not must empty.");

            this.RuleFor(command => command.Customer)
                .NotEmpty()
                    .WithMessage("The Customer not must empty.");
        }
    }
}
