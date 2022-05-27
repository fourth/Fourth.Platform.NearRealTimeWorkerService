using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Primitives;
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
            var consumer = new EventHubConsumerClient(consumerGroup,connectionString,eventHubName);

            using CancellationTokenSource cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(30));


            string firstPartition;

            await using (var producer = new EventHubProducerClient(connectionString, eventHubName))
            {
                firstPartition = (await producer.GetPartitionIdsAsync()).First();
            }

            var receiver = new PartitionReceiver(
                consumerGroup,
                firstPartition,
                EventPosition.Earliest,
                connectionString,
                eventHubName);

            try
            {
                while (!cancellationSource.IsCancellationRequested)
                {
                    int batchSize = 50;
                    TimeSpan waitTime = TimeSpan.FromSeconds(1);

                    IEnumerable<EventData> eventBatch = await receiver.ReceiveBatchAsync(
                        batchSize,
                        waitTime,
                        cancellationSource.Token);

                    foreach (EventData eventData in eventBatch)
                    {
                        byte[] eventBodyBytes = eventData.EventBody.ToArray();
                        Console.WriteLine($"Read event of length { eventBodyBytes.Length } from { firstPartition }");
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // This is expected if the cancellation token is
                // signaled.
            }
            finally
            {
                await receiver.CloseAsync();
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