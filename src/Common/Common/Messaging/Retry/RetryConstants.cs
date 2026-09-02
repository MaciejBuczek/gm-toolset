namespace Common.Messaging.Retry
{
    public static class RetryConstants
    {
        public const string RetryExchangeName = "gm-toolset.retry";
        public const string RetryTTLArgumentName = "x-message-ttl";
        public const string RetryDeadLetterExchangeArgumentName = "x-dead-letter-exchange";
        public const string RetryDeadLetterRoutingKeyArgumentName = "x-dead-letter-routing-key";
        public const string RetryCountHeaderName = "x-retry-count";

    }
}
