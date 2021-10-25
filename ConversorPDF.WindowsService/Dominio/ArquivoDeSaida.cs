using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;

namespace ConversorPDF.WindowsService.Dominio
{
    public class ArquivoDeSaida
    {
        private readonly string arquivo;
        private readonly string diretorio;
        private readonly string conteudo;


        public ArquivoDeSaida(string diretorio, string arquivo, string conteudo)
        {
            this.diretorio = diretorio;
            this.arquivo = arquivo;
            this.conteudo = conteudo;
        }

        public bool GerarArquivoPdf(out string nomeArquivo)
        {
            nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);

            try
            {
                var nome = string.Format(@"{0}\{1}.pdf", diretorio, nomeArquivo);

                using var fileStream = new FileStream(nome, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                using PdfWriter writer = new PdfWriter(fileStream);

                using PdfDocument pdf = new PdfDocument(writer);

                using Document document = new Document(pdf);

                Paragraph texto = new Paragraph(conteudo)
                  .SetTextAlignment(TextAlignment.LEFT);

                document.Add(texto);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
