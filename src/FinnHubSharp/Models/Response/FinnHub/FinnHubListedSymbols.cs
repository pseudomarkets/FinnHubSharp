using System.Collections.Generic;
using FinnHubSharp.Models.Response.Raw;

namespace FinnHubSharp.Models.Response.FinnHub
{
    public class FinnHubListedSymbols : FinnHubResponseBase
    {
        public IEnumerable<ListedSymbol> ListedSymbols { get; set; }
    }
}