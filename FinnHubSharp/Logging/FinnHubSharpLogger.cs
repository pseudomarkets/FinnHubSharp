using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;

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