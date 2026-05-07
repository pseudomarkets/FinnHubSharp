using System.Collections.Generic;
using System.Threading.Tasks;
using FinnHubSharp.DataModels.Response;
using FinnHubSharp.DataModels.Response.FinnHub;

namespace FinnHubSharp.Interfaces
{
    public interface IFinnHubClient
    {
        Task<FinnHubQuote> GetQuoteAsync(string symbol);

        Task<FinnHubSymbolInfo> GetSymbolInfoAsync(string symbolOrSecurityName);

        Task<FinnHubListedSymbols> GetAllSymbolsAsync(string exchange);
    }
}