using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using Fourth.Platform.RealTimeWorkerService.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fourth.Platform.RealTimeWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string connectionString = "Endpoint=sb://fourthpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=82VlwEsVjhf7Sg1GAO2YAwtbw/RPtu3g/KK8Kg4tEu0=";
        private const string eventHubName = "realtime";
        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using (var consumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
                {
                    EventPosition startingPosition = EventPosition.Earliest;
                    string partitionId = (await consumer.GetPartitionIdsAsync()).First();

                    using var cancellationSource = new CancellationTokenSource();
                    cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

                    await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
                    {
                        // At this point, the loop will wait for events to be available in the Event Hub.  When an event
                        // is available, the loop will iterate with the event that was received.  Because we did not
                        // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
                        // the cancellation token.
                    }
                }

                await base.StartAsync(cancellationToken);
            }
            catch (EventHubsException ex) when
                (ex.Reason == EventHubsException.FailureReason.ConsumerDisconnected)
            {
                // Take action based on a consumer being disconnected
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }

            //todo GetSales
        }

        public async Task GetSales()
        {
            //todo GetSales
        }

        public override void Dispose()
        {
        }
    }
}