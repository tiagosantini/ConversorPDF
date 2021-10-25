using System;
using System.Collections.Generic;

namespace ConversorPDF.WindowsService.Dominio
{
    public class ProcessoDeConversao
    {
        private Guid id;
        private List<ArquivoDeEntrada> arquivosDeEntrada;

        public Guid Id => id;
        public List<ArquivoDeEntrada> ArquivosDeEntrada => arquivosDeEntrada;

        public ProcessoDeConversao(Guid id, List<ArquivoDeEntrada> arquivosDeEntrada)
        {
            this.id = id;
            this.arquivosDeEntrada = arquivosDeEntrada;
        }
    }
}
