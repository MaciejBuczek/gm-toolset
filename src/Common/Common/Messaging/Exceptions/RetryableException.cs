namespace Common.Messaging.Exceptions
{
    public class RetryableException : Exception
    {
        public string? Details { get; set; }

        public RetryableException(string message) : base(message)
        {
        }

        public RetryableException(string message, string details) : base(message)
        {
            Details = details;
        }
    }
}
