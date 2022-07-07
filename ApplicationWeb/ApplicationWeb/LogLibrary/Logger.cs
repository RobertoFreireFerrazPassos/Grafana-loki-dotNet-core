using Serilog;
using System;
using System.Text.Json;

namespace LogLibrary
{
    public class ExceptionData
    {
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
    public class LogData
    {
        public object Content { get; set; }
        public ExceptionData Exception { get; set; }
        public string Message { get; set; }
        public string Context { get; set; }
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
        public static void Information(object content, string context, string message = "")
        {
            var dataLog = new LogData() { 
                Content = content,
                Context = context,
                Message = message 
            };

            Log.Information(JsonSerializer.Serialize(dataLog));
        }

        public static void Error(Exception ex, string context, string message = "")
        {
            var dataLog = new LogData() { 
                Exception = new ExceptionData()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                },
                Context = context,
                Message = message 
             };

            Log.Information(JsonSerializer.Serialize(dataLog));
        }
    }
}