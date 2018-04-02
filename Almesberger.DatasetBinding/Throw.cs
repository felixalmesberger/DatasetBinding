using System;

namespace System.Windows.Forms.More.DatasetBinding
{
  internal static class Throw
  {
    public static void IfNull(object value, string name)
    {
      if (value == null)
        throw new ArgumentNullException(name);
    }

    public static void IfNull<T>(object value, string message)
      where T : Exception
    {
      if (value == null)
        throw ActivateException<T>(message);
    }

    public static void If(bool condition, string message)
    {
      if (condition)
        throw new Exception(message);
    }

    public static void If<T>(bool condition, string message)
      where T : Exception
    {
      if (condition)
        throw ActivateException<T>(message);
    }

    public static void IfNot(bool condition, string message)
    {
      if (!condition)
        throw new Exception(message);
    }

    public static void IfNot<T>(bool condition, string message)
      where T : Exception
    {
      if (!condition)
        throw ActivateException<T>(message);
    }



    private static Exception ActivateException<T>(string message, Exception innerException = null)
      where T : Exception
    {
      return (Exception)Activator.CreateInstance(typeof(T), new[] { (object)message, (object)innerException });
    }
  }
}