using NLog;
using System.Collections.Concurrent;

namespace CS_Core
{
    /// <summary>
    /// Logger service
    /// </summary>
    public static class LogService
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Log info
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="methodName"></param>
        /// <param name="msg"></param>
        public static void Info(string objectName, string methodName, string msg)
        {
            if (Logger.IsInfoEnabled) Logger.Info($"{objectName}.{methodName} | {msg}");
        }

        /// <summary>
        /// Log trace
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="msg"></param>
        public static void Trace(string objectName, string msg)
        {
            if (Logger.IsTraceEnabled) Logger.Trace($"{objectName} | {msg}");
        }

        /// <summary>
        /// Log warn
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="methodName"></param>
        /// <param name="msg"></param>
        public static void Warn(string objectName, string methodName, string msg)
        {
            if (Logger.IsWarnEnabled) Logger.Warn($"{objectName}.{methodName} | {msg}");
        }

        /// <summary>
        /// Log Fatal
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="objectName"></param>
        /// <param name="methodName"></param>
        /// <param name="msg"></param>
        public static void Fatal(Exception ex, string objectName, string methodName, string msg)
        {
            if (Logger.IsFatalEnabled) Logger.Fatal(ex, $"{objectName}.{methodName} | {msg}");
        }

        /// <summary>
        /// Log Fatal
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="objectName"></param>
        /// <param name="methodName"></param>
        /// <param name="msg"></param>
        public static void Fatal(Exception ex, string objectName, string methodName)
        {
            if (Logger.IsFatalEnabled) Logger.Fatal(ex, $"{objectName}.{methodName}");
        }
    }
}
