namespace Identity.API.Features.Login
{
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username or Email is required")
                .When(x => string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Username or Email is required")
                .EmailAddress()
                .When(x => string.IsNullOrEmpty(x.Username));
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
