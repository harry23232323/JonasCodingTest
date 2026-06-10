using System;

namespace BusinessLayer.Model.Interfaces
{
    public interface IAppLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception = null);
    }
}
