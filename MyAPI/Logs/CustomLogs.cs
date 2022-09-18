using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAPI.Logs
{
    public class CustomLogs
    {
        const string PUT = "put";
        const string PATCH = "patch";
        const string DELETE = "delete";

        public static void SaveLog(int id,
            string message,
            string title,
            string artist,
            string method,
            object? entityBefore = null, // por que nullable?
            object? entityAfter = null) // por que nullable?
        {
            var now = DateTime.Now.ToString("G");

            if (method.Equals(PUT, StringComparison.InvariantCultureIgnoreCase)
                || method.Equals(PATCH, StringComparison.InvariantCultureIgnoreCase))
            {
                var entityBeforeJson = JsonConvert.SerializeObject(entityBefore);
                var entityBeforeJsonFormatted = JValue.Parse(entityBeforeJson).ToString(Formatting.Indented);

                var entityAfterJson = JsonConvert.SerializeObject(entityAfter);
                var entityAfterJsonFormatted = JValue.Parse(entityAfterJson).ToString(Formatting.Indented);

                Console.WriteLine($"{now} - {message} - {id} - {title} - {artist} - " +
                    $"Alterado de {entityBeforeJsonFormatted} " +
                    $"\npara {entityAfterJsonFormatted}.");
            }
            else if (method.Equals(DELETE, StringComparison.InvariantCultureIgnoreCase)) 
            {
                Console.WriteLine($"{now} - {message} - {id} - {title} - {artist} - Removida.");
            }
        }
    }
}