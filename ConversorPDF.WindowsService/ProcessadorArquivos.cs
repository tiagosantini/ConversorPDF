using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ConversorPDF.Config;
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
                string arquivoSaidaPdf = a.Name.Replace(".filter", ".pdf");

                string caminhoSaida = $"{Configuracao.Saida}{arquivoSaidaPdf}";

                using var fileStream = new FileStream(caminhoSaida, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                using PdfWriter writer = new PdfWriter(fileStream);

                using PdfDocument pdf = new PdfDocument(writer);

                using Document document = new Document(pdf);

                Paragraph texto = new Paragraph("Teste")
                   .SetTextAlignment(TextAlignment.LEFT);

                document.Add(texto);

                a.Delete();

                Log.Information($"Arquivo: {a.Name} convertido para PDF com sucesso!");
            }
        }

        //public bool GerarArquivoPdf(out string nomeArquivo)
        //{
        //    nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);

        //    try
        //    {
        //        var nome = string.Format(@"{0}{1}.pdf", diretorio, nomeArquivo);

        //        using var fileStream = new FileStream(nome, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

        //        using PdfWriter writer = new PdfWriter(fileStream);

        //        using PdfDocument pdf = new PdfDocument(writer);

        //        using Document document = new Document(pdf);

        //        Paragraph texto = new Paragraph(conteudo)
        //           .SetTextAlignment(TextAlignment.LEFT);

        //        document.Add(texto);
        //    }
        //    catch (System.Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
