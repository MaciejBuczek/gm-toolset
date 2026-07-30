namespace Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public string? Details { get; set; }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, string details) : base(message)
        {
            Details = details;
        }
    }
}
