namespace Identity.API.Exceptions
{
    public class IdentityException : InternalServerException
    {
        public IdentityException(string message) : base(message)
        {
        }

        public IdentityException(string message, string details) : base(message, details)
        {
        }
    }
}
