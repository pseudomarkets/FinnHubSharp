using System.Collections.Generic;
using System.Threading.Tasks;
using FinnHubSharp.DataModels.Response;

namespace FinnHubSharp.Interfaces
{
    public interface IFinnHubClient
    {
        Task<Quote> GetQuoteAsync(string symbol);

        Task<SymbolInfo> GetSymbolInfoAsync(string symbolOrSecurityName);

        Task<IEnumerable<ListedSymbol>> GetAllSymbols(string exchange);
    }
}