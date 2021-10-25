using ConversorPDF.Config;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ConversorPDF.WindowsService.Dominio;
using System.Diagnostics;

namespace ConversorPDF.Workers
{
    public class ProcessadorDeArquivosWorker : BackgroundService
    {
        public FilaDeConversao FilaDeConversao { get; init; }
        public Configuracao Configuracao { get; init; }

        public ProcessadorDeArquivosWorker(Configuracao config, FilaDeConversao filaDeConversao)
        {
            Configuracao = config;
            FilaDeConversao = filaDeConversao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Log.Debug("Buscando processos de conversão...");

                ProcessoDeConversao processoSelecionado = FilaDeConversao.ObterProcessoDaFila();

                if (processoSelecionado == null)
                {
                    Log.Debug("Nenhum processo foi encontrado. Buscando novamente em 5 segundos.");

                    await Task.Delay(5000);
                    continue;
                }

                try
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();

                    Log.Information("Iniciando o processo de conversão GUID [{Processo}] com {QtdArquivos} arquivos...", processoSelecionado.Id, processoSelecionado.ArquivosDeEntrada.Count);

                    Parallel.ForEach(processoSelecionado.ArquivosDeEntrada, (arquivoDeEntrada) =>
                    {
                        ArquivoDeSaida arquivoDeSaida = new ArquivoDeSaida(Configuracao.Saida, arquivoDeEntrada.Nome, arquivoDeEntrada.Conteudo);

                        string nomeDoArquivo;

                        bool pdfCriado = arquivoDeSaida.GerarArquivoPdf(out nomeDoArquivo);

                        if (pdfCriado)
                        {
                            Log.Information("{NomeArquivo} gerado com sucesso!", Path.GetFileName(nomeDoArquivo));

                            if (arquivoDeEntrada.MoverPara(Configuracao.Processados) == false)
                                Log.Warning("{Arquivo} não foi movido para {Diretorio}...", arquivoDeEntrada.Nome, Configuracao.Saida);
                        }
                        else if (arquivoDeEntrada.MoverPara(Configuracao.Falha) == false)
                            Log.Warning("{Arquivo} não foi movido para {Diretorio}...", arquivoDeEntrada.Nome, Configuracao.Saida);

                        else
                            Log.Error("Falha ao processar o arquivo '{Arquivo}'", arquivoDeEntrada.Nome);
                    });

                    watch.Stop();

                    Log.Information("O processo de conversão GUID [{Processo}] foi finalizado em {Tempo} segundos.",
                        processoSelecionado.Id, watch.ElapsedMilliseconds / 1000.0);
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex, "{ProcessoSelecionado}", processoSelecionado);
                }
            }
        }
    }
}
