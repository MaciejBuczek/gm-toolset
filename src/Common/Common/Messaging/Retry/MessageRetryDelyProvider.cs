namespace Common.Messaging.Retry
{
    public class MessageRetryDelyProvider
    {
        private static readonly TimeSpan[] RetryDelays =
        [
            TimeSpan.FromSeconds(10),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(10)
        ];

        public static bool TryGetRetryDelay(int retryCount, out TimeSpan delay)
        {
            if (retryCount >= RetryDelays.Length)
            {
                delay = default;
                return false;
            }
            if(retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), "Retry count cannot be negative.");
            }

            delay = RetryDelays[retryCount];
            return true;
        }
    }
}
