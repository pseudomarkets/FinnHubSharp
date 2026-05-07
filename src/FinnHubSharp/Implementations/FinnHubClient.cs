using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinnHubSharp.Interfaces;
using FinnHubSharp.Models.Response.FinnHub;
using FinnHubSharp.Models.Response.Raw;
using System.Text.Json;
using FinnHubSharp.Models.Configuration;

namespace FinnHubSharp.Implementations
{
    public class FinnHubClient : IFinnHubClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public FinnHubClient(HttpClient client, FinnHubSharpConfiguration configuration)
        {
            _client = client ?? new HttpClient();
            _apiKey = configuration.ApiKey ?? throw new ArgumentNullException(nameof(configuration.ApiKey));
            _baseUrl = configuration.BaseUrl ?? throw new ArgumentNullException(nameof(configuration.BaseUrl));
        }

        public async Task<FinnHubQuote> GetQuoteAsync(string symbol)
        {
            var quote = new FinnHubQuote();
            
            try
            {
                var endpoint = $"{_baseUrl}/quote?symbol={symbol}&token={_apiKey}";
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                
                var jsonResponse = JsonSerializer.Deserialize<Quote>(responseString);
                quote.Quote = jsonResponse;
                quote.ResponseCode = (int)response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    quote.ErrorMessage = responseString;
                }
                
                return quote;
            }
            catch (Exception e)
            {
                quote.ErrorMessage = e.ToString();
            }

            return quote;
        }

        public async Task<FinnHubSymbolInfo> GetSymbolInfoAsync(string symbolOrSecurityName)
        {
            var symbolInfo = new FinnHubSymbolInfo();
            try
            {
                var endpoint = $"{_baseUrl}/search?q={symbolOrSecurityName}&token={_apiKey}";
                var response = await _client.GetAsync(endpoint);
                
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<SymbolInfo>(responseString);
                
                symbolInfo.SymbolInfo = jsonResponse;
                symbolInfo.ResponseCode = (int)response.StatusCode;
                
                if (!response.IsSuccessStatusCode)
                {
                    symbolInfo.ErrorMessage = responseString;
                }
            }
            catch (Exception e)
            {
                symbolInfo.ErrorMessage = e.ToString();
            }
            
            return symbolInfo;
        }
        
        public async Task<FinnHubListedSymbols> GetAllSymbolsAsync(string exchange)
        {
            var listedSymbols = new FinnHubListedSymbols();
            
            try
            {
                var endpoint = $"{_baseUrl}/stock/symbol?exchange={exchange}&token={_apiKey}";
                var response = await _client.GetAsync(endpoint);
                
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<List<ListedSymbol>>(responseString);

                listedSymbols.ListedSymbols = jsonResponse;
                listedSymbols.ResponseCode = (int)response.StatusCode;
                
                if (!response.IsSuccessStatusCode)
                {
                    listedSymbols.ErrorMessage = responseString;
                }
                
            }
            catch (Exception e)
            {
                listedSymbols.ErrorMessage = e.ToString();
            }
            
            return listedSymbols;
        }
    }
}
