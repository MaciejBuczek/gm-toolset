namespace Identity.API.Features.LoginUsingRefreshToken
{
    internal class LoginUsingRefreshTokenCommandValidator : AbstractValidator<LoginUsingRefreshTokenCommand>
    {
        public LoginUsingRefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}
