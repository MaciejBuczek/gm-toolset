namespace Common.Messaging.Retry
{
    public static class RetryConstants
    {
        public const string RetryExchangeName = "gm-toolset.retry";
        public const string RetryTTLHeaderName = "x-message-ttl";
        public const string RetryDeadLetterExchangeHeaderName = "x-dead-letter-exchange";
        public const string RetryDeadLetterRoutingKeyHeaderName = "x-dead-letter-routing-key";
        public const string RetryCountHeaderName = "x-retry-count";

    }
}
