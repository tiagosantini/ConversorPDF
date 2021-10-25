using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorPDF.WindowsService.Dominio
{
    public class ArquivoDeEntrada
    {

        public ArquivoDeEntrada(FileInfo fileInfo)
        {
            FileInfo = fileInfo;

            Nome = RenomeaComIncremento(fileInfo);

            fileInfo.MoveTo(Nome);

            using (var stream = new StreamReader(Nome))
            {
                Conteudo = stream.ReadToEnd();
            }
        }

        public FileInfo FileInfo { get; private set; }

        public string Nome { get; private set; }

        public string Conteudo { get; private set; }

        public bool MoverPara(string diretorio)
        {
            try
            {
                string arquivoComCaminho = string.Format(@"{0}\{1}", diretorio, FileInfo.Name);

                FileInfo.MoveTo(arquivoComCaminho, true);

                return true;

            }
            catch(Exception)
            {
                return false;
            }
        }

        private string RenomeaComIncremento(FileInfo fileInfo)
        {
            string novoNome = string.Format(@"{0}\{1}.filter", fileInfo.Directory.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name));

            int i = 1;

            while (File.Exists(novoNome))
            {
                novoNome = string.Format("{0}({1}).filter", fileInfo.FullName, i);
                i++;
            }

            return novoNome;
        }
    }
}
