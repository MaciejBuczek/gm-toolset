namespace Identity.API.Exceptions
{
    public class IdentityRoleNotFoundException : InternalServerException
    {
        public IdentityRoleNotFoundException(string message) : base(message)
        {
        }

        public IdentityRoleNotFoundException(string message, string details) : base(message, details)
        {
        }
    }
}
