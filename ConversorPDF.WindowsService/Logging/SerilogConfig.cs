using Serilog;

namespace ConversorPDF.Logging
{
    public static class SerilogConfig
    {
        public static void CriarLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
