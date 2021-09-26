using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinnHubSharp.DataModels.Response;
using FinnHubSharp.Interfaces;
using FinnHubSharp.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FinnHubSharp.Implementations
{
    public class FinnHubClient : IFinnHubClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public FinnHubClient(HttpClient client, string apiKey, ILogger logger)
        {
            _client = client ?? new HttpClient();
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _logger = logger ?? new Logger<FinnHubSharpLogger>(new LoggerFactory());
        }

        public async Task<Quote> GetQuoteAsync(string symbol)
        {
            try
            {
                var endpoint = $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={_apiKey}";
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation(FinnHubSharpLogger.BuildHttpLogUsing(response));
                
                var jsonResponse = JsonConvert.DeserializeObject<Quote>(responseString);
                return jsonResponse;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        public async Task<SymbolInfo> GetSymbolInfoAsync(string symbolOrSecurityName)
        {
            try
            {
                var endpoint = $"https://finnhub.io/api/v1/search?q={symbolOrSecurityName}&token={_apiKey}";
                var response = await _client.GetAsync(endpoint);
                
                _logger.LogInformation(FinnHubSharpLogger.BuildHttpLogUsing(response));
                
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<SymbolInfo>(responseString);
                return jsonResponse;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<ListedSymbol>> GetAllSymbols(string exchange)
        {
            try
            {
                var endpoint = $"https://finnhub.io/api/v1/stock/symbol?exchange={exchange}&token={_apiKey}";
                var response = await _client.GetAsync(endpoint);
                
                _logger.LogInformation(FinnHubSharpLogger.BuildHttpLogUsing(response));
                
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<List<ListedSymbol>>(responseString);
                return jsonResponse;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}
