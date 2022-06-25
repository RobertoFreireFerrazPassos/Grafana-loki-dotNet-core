using Serilog;
using System.Text.Json;

namespace LogLibrary
{
    public class LogData
    {
        public object Content { get; set; }

        public object Error { get; set; }

        public string LogId = Guid.NewGuid().ToString();

        public DateTime TimeStamp = DateTime.UtcNow;
    }

    public static class Logger
    {
        public static void Information(object content)
        {
            var dataLog = new LogData() { Content = content };

            Log.ForContext("AdditionalData", JsonSerializer.Serialize(dataLog))
               .Information(JsonSerializer.Serialize(content));
        }

        public static void Information(object content, string message)
        {
            var dataLog = new LogData() { Content = content };

            Log.ForContext("AdditionalData", JsonSerializer.Serialize(dataLog))               
               .Information(message);
        }

        public static void Error(object error, string message)
        {
            var dataLog = new LogData() { Error = error };

            Log.ForContext("AdditionalData", JsonSerializer.Serialize(dataLog))
               .Information(message);
        }

        public static void Error(Exception ex, object error, string message)
        {
            var dataLog = new LogData() { Error = error };

            Log.ForContext("AdditionalData", JsonSerializer.Serialize(dataLog))               
               .Error(ex, message);
        }
    }
}