using System;
using System.Diagnostics;
using BusinessLayer.Model.Interfaces;

namespace WebApi.Logging
{
    public class AppLogger : IAppLogger
    {
        private const string Source = "JonasCodingTest";

        public void LogInfo(string message)
        {
            Trace.TraceInformation($"[INFO] {Source}: {message}");
        }

        public void LogWarning(string message)
        {
            Trace.TraceWarning($"[WARN] {Source}: {message}");
        }

        public void LogError(string message, Exception exception = null)
        {
            if (exception != null)
                Trace.TraceError($"[ERROR] {Source}: {message} - {exception}");
            else
                Trace.TraceError($"[ERROR] {Source}: {message}");
        }
    }
}
