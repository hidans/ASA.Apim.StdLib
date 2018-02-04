using log4net;
using System;

namespace ASA.Apim.Library.Services
{
    internal class LogService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static void Fatal(string message, Exception exception)
        {
            log.Fatal(message, exception);
        }
        internal static void Fatal(string message)
        {
            log.Fatal(message);
        }

        internal static void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }
        internal static void Error(string message)
        {
            log.Error(message);
        }

        internal static void Warning(string message, Exception exception)
        {
            log.Warn(message, exception);
        }
        internal static void Warning(string message)
        {
            log.Warn(message);
        }

        internal static void Info(string message, Exception exception)
        {
            log.Info(message, exception);
        }
        internal static void Info(string message)
        {
            log.Info(message);
        }

        internal static void Debug(string message, Exception exception)
        {
            log.Debug(message, exception);
        }
        internal static void Debug(string message)
        {
            log.Debug(message);
        }
    }
}