using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConversorPDF.Config
{
    public static class Configuracao
    {
        public static string Entrada { get; set; }
        public static string Processamento { get; set; }
        public static string Saida { get; set; }
        public static string Falha { get; set; }

        static Configuracao()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfigurationSection diretorios = config.Build().GetSection("Diretorios");

            Entrada = diretorios["Entrada"];
            Processamento = diretorios["Processados"];
            Saida = diretorios["Saida"];
            Falha = diretorios["Falha"];
        }
    }
}
