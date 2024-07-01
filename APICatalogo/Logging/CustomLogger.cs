
namespace APICatalogo.Logging;

public class CustomLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string msg = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(msg);
    }

    private void EscreverTextoNoArquivo(string msg)
    {
        string caminhoArqLog = @"C:\EstudoDotNET\log\CursoWebLog.txt";
        using (StreamWriter streamWriter = new StreamWriter(caminhoArqLog, true))
        {
            try
            {
                streamWriter.WriteLine(msg);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
