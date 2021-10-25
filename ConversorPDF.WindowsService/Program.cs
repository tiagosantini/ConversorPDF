using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConversorPDF.Config;
using ConversorPDF.Logging;
using ConversorPDF.WindowsService.Dominio;
using ConversorPDF.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

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
                .ConfigureAppConfiguration(config =>
                    config.AddJsonFile("appsettings.json")
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build()
                )
                .UseWindowsService()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>( (hostContext, builder) =>
                {
                    IConfiguration config = hostContext.Configuration;

                    string configDiretorioRaiz = config.GetSection("Configuracao:DiretorioRaiz").Value;

                    builder.RegisterType<Configuracao>()
                        .WithParameter("diretorioRaiz", configDiretorioRaiz)
                        .SingleInstance();

                    builder.RegisterType<FilaDeConversao>().SingleInstance();
                })
                .ConfigureServices( (hostContext, services) =>
                {
                    services.AddLogging(config => config.AddSerilog(Log.Logger));
                    services.AddHostedService<ObservadorDeEntradaWorker>();
                    services.AddHostedService<ProcessadorDeArquivosWorker>();
                });
    }
}
