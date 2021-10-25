using ConversorPDF.Config;
using ConversorPDF.WindowsService.Dominio;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConversorPDF.Workers
{
    public class ObservadorDeEntradaWorker : BackgroundService
    {
        public FilaDeConversao FilaDeConversao { get; init; }

        public ObservadorDeEntradaWorker(FilaDeConversao filaDeConversao)
        {
            FilaDeConversao = filaDeConversao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information($"Observando o diretório {Configuracao.Entrada}");

            var diretorioDeEntrada = new DirectoryInfo(Configuracao.Entrada);

            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();

                Log.Debug("Selecionando arquivos do diretório de entrada...");

                stopwatch.Start();

                List<ArquivoDeEntrada> arquivosSelecionados = diretorioDeEntrada
                    .GetFiles("*.txt", SearchOption.TopDirectoryOnly)
                    .AsParallel()
                    .Select(fileInfo => new ArquivoDeEntrada(fileInfo))
                    .ToList();

                stopwatch.Stop();

                if (arquivosSelecionados.Count == 0)
                {
                    Log.Debug("Não há arquivos para selecionar, tentando novamente em 5 segundos...");

                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                Guid id = Guid.NewGuid();

                ProcessoDeConversao processoDeConversao = new ProcessoDeConversao(id, arquivosSelecionados);

                Log.Information("Seleção concluída em {Tempo} segundos", stopwatch.ElapsedMilliseconds / 1000.0);

                Log.Information("Processo GUID [{Processo}]: {QtdArquivos} arquivos capturados", id, arquivosSelecionados.Count);

                FilaDeConversao.RegistrarProcesso(processoDeConversao);
            }
        }
    }
}
