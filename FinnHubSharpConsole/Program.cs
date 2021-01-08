using System;
using System.Threading.Tasks;
using FinnHubSharp;

namespace FinnHubSharpConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter FinnHub API Key: ");
            string apiKey = Console.ReadLine();

            FinnHubClient client = new FinnHubClient(apiKey);
            var price = TestQuoteApi(client).GetAwaiter().GetResult();
            Console.WriteLine("Current price for AAPL: " + price);
        }

        public static async Task<double> TestQuoteApi(FinnHubClient client)
        {
            var quote = await client.GetQuoteAsync("AAPL");
            var price = quote.CurrentPrice;
            return price;
        }
    }
}
