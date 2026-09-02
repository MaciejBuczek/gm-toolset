namespace Common.Messaging.Retry
{
    public class RetryHeaderHandler
    {
        public static int GetRetryCount(IDictionary<string, object> headers)
        {
            return !headers.TryGetValue(RetryConstants.RetryCountHeaderName, out var value)
                ? 0
                : value switch
                    {
                        int count => count,
                        long count => (int)count,
                        byte count => count,
                        _ when int.TryParse(value?.ToString(), out var count) => count,
                        _ => 0
                    };
        }

        public static void SetRetryCount(IDictionary<string, object> headers, int count)
        {
            headers[RetryConstants.RetryCountHeaderName] = count;
        }
    }
}
