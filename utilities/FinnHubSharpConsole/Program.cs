using System;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;
using FinnHubSharp.DataModels.Configuration;
using FinnHubSharp.DataModels.Request;
using FinnHubSharp.Implementations;
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
    
            var client = new FinnHubClient(httpClient, new FinnHubSharpConfiguration() {ApiKey = apiKey});
            var quote = await client.GetQuoteAsync("AAPL");
            Console.WriteLine("Current price for AAPL: " + quote.Quote.CurrentPrice);

            var symbolInfo = await client.GetSymbolInfoAsync("Apple");
            symbolInfo.SymbolInfo.Result.ForEach(x => Console.WriteLine(x.Description));

            var allSymbols = await client.GetAllSymbolsAsync("US");
            allSymbols.ListedSymbols.ToList().ForEach(x => Console.WriteLine(x.Symbol));
            
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
