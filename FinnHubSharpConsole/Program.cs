using System;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using FinnHubSharp;
using FinnHubSharp.DataModels.Request;
using FinnHubSharp.Implementations;
using FinnHubSharp.Interfaces;
using FinnHubSharp.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace FinnHubSharpConsole
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            
            Console.Write("Enter FinnHub API Key: ");
            string apiKey = Console.ReadLine();
    
            var client = new FinnHubClient(httpClient, apiKey, new NullLogger<FinnHubSharpLogger>());
            var quote = await client.GetQuoteAsync("AAPL");
            Console.WriteLine("Current price for AAPL: " + quote.CurrentPrice);

            var symbolInfo = await client.GetSymbolInfoAsync("Apple");
            symbolInfo.Result.ForEach(x => Console.WriteLine(x.Description));

            var allSymbols = await client.GetAllSymbols("US");
            allSymbols.ToList().ForEach(x => Console.WriteLine(x.Symbol));
            
            var streamerClient = new FinnHubStreamerClient(new ClientWebSocket(), apiKey);
            
            await foreach (var data in streamerClient.GetStreamingQuotes(new StreamingSubscription()
                {Type = "subscribe", Symbol = "BINANCE:BTCUSDT"}))
            {
                await foreach (var nextData in data)
                {
                    nextData?.Data?.ForEach(x => Console.WriteLine(JsonConvert.SerializeObject(x)));
                }
            }
        }
    }
}
