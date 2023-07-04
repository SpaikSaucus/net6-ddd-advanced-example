using AuthorizationOperation.Application.UserCases.Create.Command;
using FluentValidation;

namespace AuthorizationOperation.Application.UserCases.Create.Validations
{
    public class CreateAuthorizationValidation : AbstractValidator<CreateAuthorizationCommand>
    {
        public CreateAuthorizationValidation()
        {
            this.RuleFor(command => command.Request.UUID)
                .NotEmpty()
                    .WithMessage("The UUID not must empty.");

            this.RuleFor(command => command.Request.Customer)
                .NotEmpty()
                    .WithMessage("The Customer not must empty.");
        }
    }
}
