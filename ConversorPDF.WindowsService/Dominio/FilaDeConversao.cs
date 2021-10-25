using System.Collections.Concurrent;

namespace ConversorPDF.WindowsService.Dominio
{
    public class FilaDeConversao
    {
        private readonly ConcurrentBag<ProcessoDeConversao> processos;

        public ConcurrentBag<ProcessoDeConversao> Processos { get => processos; }

        public FilaDeConversao()
        {
            processos = new ConcurrentBag<ProcessoDeConversao>();
        }

        public ProcessoDeConversao ObterProcessoDaFila()
        {
            try
            {
                ProcessoDeConversao processoSelecionado;

                processos.TryTake(out processoSelecionado);

                return processoSelecionado;
            }
            catch
            {
                return null;
            }
        }

        public void RegistrarProcesso(ProcessoDeConversao novoProcesso) => processos.Add(novoProcesso);
    }
}
