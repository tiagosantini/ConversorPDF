using System.IO;

namespace ConversorPDF.Config
{
    public class Configuracao
    {
        public string DiretorioRaiz { get; init; }
        public string Entrada => VerificaSeExisteEhCriaDiretorio("Arquivos TXT");
        public string Processados => VerificaSeExisteEhCriaDiretorio("Arquivos Processados");
        public string Saida => VerificaSeExisteEhCriaDiretorio("Arquivos PDF");
        public string Falha => VerificaSeExisteEhCriaDiretorio("Arquivos com Falha");

        public Configuracao(string diretorioRaiz)
        {
            DiretorioRaiz = diretorioRaiz;
        }

        private string VerificaSeExisteEhCriaDiretorio(string diretorio)
        {
            var caminho = Path.Combine(DiretorioRaiz, diretorio);

            if (Directory.Exists(caminho) == false)
                Directory.CreateDirectory(caminho);

            return caminho;
        }
    }
}
