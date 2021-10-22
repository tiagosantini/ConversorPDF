using ConversorPDF.Config;
using ConversorPDF.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConversorPDF
{
    public class ObservadorEntrada : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information($"Observando o diretório {Configuracao.Entrada}");

            while (!stoppingToken.IsCancellationRequested)
            {
                FileInfo[] arquivosRenomeados = await Task.Run( () => ObservarDiretorioEntrada(), stoppingToken);

                EnviarParaProcessamento(arquivosRenomeados);

                await Task.Delay(1000, stoppingToken);
            }
        }

        private FileInfo[] ObservarDiretorioEntrada()
        {
            var arquivos = new DirectoryInfo(Configuracao.Entrada).GetFiles("*.txt");

            Parallel.ForEach(arquivos, (arquivo) => RenomearArquivo(arquivo));

            return arquivos;
        }

        private FileInfo RenomearArquivo(FileInfo arquivo)
        {
            string nomeInicial = arquivo.Name;

            //arquivo.RenameWithIncrement(arquivo.FullName);
            //arquivo.Name.Replace(".txt", ".filter");

            arquivo.MoveTo(Path.ChangeExtension(arquivo.FullName, ".filter"));

            Log.Information($"Renomeando arquivo: '{nomeInicial}' para '{arquivo.Name}'...");

            return arquivo;
        }

        private void EnviarParaProcessamento(FileInfo[] arquivosRenomeados)
        {
            Parallel.ForEach(arquivosRenomeados, (arquivo) => MoverArquivo(arquivo));
        }

        private void MoverArquivo(FileInfo arquivoRenomeado)
        {
            string caminhoProcessamento = $"{Configuracao.Processamento}{arquivoRenomeado.Name}";

            string caminhoFalha = $"{Configuracao.Falha}{arquivoRenomeado.Name}";

            if (!File.Exists(caminhoProcessamento))
            {
                arquivoRenomeado.MoveTo(caminhoProcessamento, false);
                Log.Information($"Arquivo: '{arquivoRenomeado.Name}' enviado para processamento...");
            }
            else
            {
                arquivoRenomeado.MoveTo(caminhoFalha, false);
                Log.Information($"Arquivo: '{arquivoRenomeado.Name}' já existe em processamento, operação não realizada...");
            }
        }
    }
}
