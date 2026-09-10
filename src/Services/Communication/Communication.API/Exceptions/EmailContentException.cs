namespace Communication.API.Exceptions
{
    public class EmailContentException : BadRequestException
    {
        public EmailContentException(string message) : base(message)
        {
        }

        public EmailContentException(string message, string details) : base(message, details)
        {
        }
    }
}
