using System.Threading.Tasks;
using FinnHubSharp.Models.Response.FinnHub;

namespace FinnHubSharp.Interfaces
{
    public interface IFinnHubClient
    {
        Task<FinnHubQuote> GetQuoteAsync(string symbol);

        Task<FinnHubSymbolInfo> GetSymbolInfoAsync(string symbolOrSecurityName);

        Task<FinnHubListedSymbols> GetAllSymbolsAsync(string exchange);
    }
}