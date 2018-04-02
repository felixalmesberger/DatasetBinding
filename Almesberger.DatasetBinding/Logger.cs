namespace System.Windows.Forms.More.DatasetBinding
{
  internal class Logger
  {

    private static readonly Action<string> NullLogger = msg => { };

    private static Action<string> LogMessage = NullLogger;

    public static void RegisterLoggingMethod(Action<string> logMessage)
    {
      LogMessage = logMessage;
    }

    public static void Log(string msg, Exception ex = null)
    {
      LogMessage?.Invoke(msg + "\n" + "Exception:" + ex);
    }

  }
}