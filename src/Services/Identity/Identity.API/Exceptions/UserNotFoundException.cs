namespace Identity.API.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string name, object key) : base(name, key)
        {
        }
    }
}
