namespace Identity.API.Register
{
    internal class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(64).MinimumLength(3);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100)
                .Must(x => x.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter.")
                .Must(x => x.Any(char.IsLower)).WithMessage("Password must contain at least one lowercase letter.")
                .Must(x => x.Any(char.IsDigit)).WithMessage("Password must contain at least one digit.")
                .Must(x => x.Any(ch => !char.IsLetterOrDigit(ch))).WithMessage("Password must contain at least one special character.");
        }
    }
}
