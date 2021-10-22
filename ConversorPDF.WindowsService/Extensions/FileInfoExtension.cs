using System.IO;

namespace ConversorPDF.Extensions
{
    public static class FileInfoExtension
    {
        public static void RenameWithIncrement(this FileInfo instance, string newName)
        {
            int i = 1;

            while (File.Exists(newName))
            {
                newName = string.Format("{0}({1}).filter", instance.FullName, i);
                i++;
            }

            instance.MoveTo(newName);
        }
    }
}
