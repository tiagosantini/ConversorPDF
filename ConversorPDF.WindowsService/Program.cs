using ConversorPDF.Logging;
using ConversorPDF.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConversorPDF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SerilogConfig.CriarLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config => config.AddSerilog(Log.Logger));
                    services.AddHostedService<ObservadorEntrada>();
                    services.AddHostedService<ProcessadorArquivos>();
                });
    }
}
