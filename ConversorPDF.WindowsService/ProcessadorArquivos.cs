using ConversorPDF.Config;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Linq;
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
                await ConverterParaPDFAsync();

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ConverterParaPDFAsync()
        {
            var arquivos = new DirectoryInfo(Configuracao.Processamento).GetFiles("*.filter");

            foreach (var a in arquivos)
            {
                Document doc = new Document();

                string caminhoSaida = $"{Configuracao.Saida}{a.Name}";

                StreamReader rdr = new StreamReader(a.FullName);

                doc.Open();

                PdfWriter.GetInstance(doc, new FileStream(caminhoSaida, FileMode.CreateNew));

                doc.Add(new Paragraph(await rdr.ReadToEndAsync()));

                doc.Close();

                Log.Information($"Arquivo: {a.Name} convertido para PDF com sucesso!");
            }
        }
    }
}
