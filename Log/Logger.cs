using Serilog;
using System.Text.Json;

namespace LogLibrary
{
    public static class Logger
    {
        public static void Information(object data)
        {
            Log.ForContext("Extra", data)
               .Information(JsonSerializer.Serialize(data));
        }

        public static void Information(object data, string message)
        {
            Log.ForContext("Extra", data)
               .Information(message);
        }

        public static void Error(Exception ex, object data, string message)
        {
            Log.ForContext("Error",data)
                  .Error(ex, message);
        }

        public static void Error(object data, string message)
        {
            Log.ForContext("Error", data)
                  .Information(message);
        }
    }
}