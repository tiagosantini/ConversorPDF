using ConversorPDF.Config;
using iTextSharp.text;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConversorPDF
{
    public class ProcessadorArquivos : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ConverterParaPDFAsync()
        {
            var arquivos = new DirectoryInfo(Configuracao.Processamento).GetFiles("*.filter");

            Document doc = new Document();

            doc.Open();

            foreach (var a in arquivos)
            {
                StreamReader rdr = new StreamReader(a.OpenRead());

                doc.Add(new Paragraph(await rdr.ReadToEndAsync()));
            }

            doc.Close();
        }
    }
}
