using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using System.Reflection;

namespace Communication.API.Services
{
    public class AutoSubscriberHostedService(IServiceProvider ServiceProvider, IBus Bus) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var autoSubscriber = new AutoSubscriber(Bus, ServiceProvider, CommunicationConstants.SubscriptionPrefixId);
            await autoSubscriber.SubscribeAsync(Assembly.GetExecutingAssembly().GetTypes(), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}