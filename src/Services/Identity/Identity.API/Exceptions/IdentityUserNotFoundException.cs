namespace Identity.API.Exceptions
{
    public class IdentityUserNotFoundException : NotFoundException
    {
        public IdentityUserNotFoundException(string message) : base(message)
        {
        }

        public IdentityUserNotFoundException(string name, object key) : base(name, key)
        {
        }
    }
}
