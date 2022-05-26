using Coravel;
using Fourth.Platform.RealTimeWorkerService;

public class Program
{
    public static void Main(string[] args)
    {

        var host = CreateHostBuilder(args).Build();
        host.Services.UseScheduler(scheduler => {
            scheduler
                .Schedule<GetSalesJobService>()
                .EveryFiveSeconds();
        });
        host.Run();

    }


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddScheduler();
                services.AddTransient<GetSalesJobService>();
                services.AddHostedService<Worker>();
            });
}
