using System.Net.Http;

namespace FinnHubSharp.Logging
{
    public class FinnHubSharpLogger
    {
        public static string BuildHttpLogUsing(HttpResponseMessage responseMessage)
        {
            return $"FinnHub API Response: {responseMessage.StatusCode.ToString()}";
        }
    }
}