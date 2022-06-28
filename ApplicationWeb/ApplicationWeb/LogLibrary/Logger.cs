using Serilog;
using System.Text.Json;

namespace LogLibrary
{
    public class LogData
    {
        public object Content { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public string LogId { get; set; }
        public DateTime TimeStamp { get; set; }

        public LogData()
        {
            LogId = Guid.NewGuid().ToString();
            TimeStamp = DateTime.UtcNow;
        }
    }

    public static class Logger
    {
        public static void Information(object content, string message = "")
        {
            var dataLog = new LogData() { Content = content, Message = message };

            Log.Information(JsonSerializer.Serialize(dataLog));
        }

        public static void Error(Exception ex, string message = "")
        {
            var dataLog = new LogData() { Exception = ex, Message = message };

            Log.Information(JsonSerializer.Serialize(dataLog));
        }
    }
}