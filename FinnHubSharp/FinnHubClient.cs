using System;
using System.Net.Http;
using System.Threading.Tasks;
using FinnHubSharp.DataModels;
using Newtonsoft.Json;

namespace FinnHubSharp
{
    public class FinnHubClient
    {
        private string _apiKey = string.Empty;

        public FinnHubClient(string apiKey)
        {
            apiKey = _apiKey;
        }

        public async Task<Quote> GetQuoteAsync(string symbol)
        {
            try
            {
                var client = new HttpClient();
                string endpoint = "https://finnhub.io/api/v1/quote?symbol=" + symbol + "&token=" + _apiKey;
                var response = await client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<Quote>(responseString);
                return jsonResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine("FinnHubSharp - GetQuoteAsync:" + e);
                return null;
            }
        }

    }
}
