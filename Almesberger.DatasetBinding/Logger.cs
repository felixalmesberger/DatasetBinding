namespace System.Windows.Forms.More.DatasetBinding
{
  internal class Logger
  {

    private static readonly Action<string> NullLogger = msg => { };

    private static Action<string> logMessage = NullLogger;

    public static void RegisterLoggingMethod(Action<string> logMessage)
    {
      Logger.logMessage = logMessage;
    }

    public static void Log(string msg, Exception ex = null)
    {
      logMessage?.Invoke(msg + "\n" + "Exception:" + ex);
    }

  }
}