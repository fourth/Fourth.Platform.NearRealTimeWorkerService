using Fourth.Platform.NearRealTimeWorkerService;
using Fourth.Platform.NearRealTimeWorkerService.Data;
using Fourth.Platform.RealTimeWorkerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{

    public const string DefaultConnection = "Server=realtimewidgets.database.windows.net;Database=widgetsdata;uid=widgetadmin;password=cJzCndnf5sR45BDrLM;";

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Run();

    }


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuraton = hostContext.Configuration;
                AppSettings.Configuration = configuraton;
                AppSettings.ConnectionString = configuraton.GetConnectionString("DBConnection");
                var optionBuilder = new DbContextOptionsBuilder<ModelContext>();
                optionBuilder.UseSqlServer(AppSettings.ConnectionString);
                services.AddSingleton(options => new ModelContext(optionBuilder.Options));

                services.AddHostedService<Worker>();
            });
}
