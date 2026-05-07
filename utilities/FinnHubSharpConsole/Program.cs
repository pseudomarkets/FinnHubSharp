using System;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using FinnHubSharp.Implementations;
using FinnHubSharp.Models.Configuration;
using FinnHubSharp.Models.Request;

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
            
            var marketStatus = await client.GetMarketStatusAsync("US");
            Console.WriteLine(marketStatus.MarketStatus.IsOpen);
            
            var marketHoliday = await client.GetMarketHolidaysAsync("US");
            Console.WriteLine(marketHoliday.MarketHoliday.Data[0].EventName);
            
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
                    nextData?.Data?.ForEach(x => Console.WriteLine(JsonSerializer.Serialize(x)));
                }
            }
        }
    }
}
