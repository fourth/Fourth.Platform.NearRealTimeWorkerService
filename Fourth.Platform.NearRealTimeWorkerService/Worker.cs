using Azure.Messaging.EventHubs.Consumer;
using Fourth.Platform.NearRealTimeWorkerService.Data;
using Fourth.Platform.NearRealTimeWorkerService.Services;
using Fourth.Platform.RealTimeWorkerService.Model;
using Newtonsoft.Json;
using System.Text;

namespace Fourth.Platform.RealTimeWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string connectionString = "Endpoint=sb://fourthpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=82VlwEsVjhf7Sg1GAO2YAwtbw/RPtu3g/KK8Kg4tEu0=";
        private const string eventHubName = "realtime";
        private const string storageConnectionString = "server=realtimewidgets.database.windows.net;database=widgetsdata;uid=widgetadmin;password=cJzCndnf5sR45BDrLM;";
        private const string blobContainerName = "<< NAME OF THE BLOBS CONTAINER >>";
        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
        private readonly DbHelper dbHelper;

        public Worker(ILogger<Worker> logger, ModelContext context)
        {
            _logger = logger;
            dbHelper = new DbHelper();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            using CancellationTokenSource cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(300));

            try
            {
                await using (var consumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
                {
                    await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
                    {
                        var dataAsJson = Encoding.UTF8.GetString(receivedEvent.Data.Body.ToArray());
                        var transactionData = JsonConvert.DeserializeObject<Transact>(dataAsJson);
                        if (transactionData != null)
                            dbHelper.SaveTransaction(transactionData);
                        else
                            _logger.LogInformation($"Empty transaction");
                    }
                }
            }

            catch (TaskCanceledException)
            {
                // This is expected if the cancellation token is
                // signaled.
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

        }

        public override void Dispose()
        {
        }
    }
}