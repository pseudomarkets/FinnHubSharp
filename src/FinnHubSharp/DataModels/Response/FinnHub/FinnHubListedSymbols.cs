using System.Collections.Generic;
using FinnHubSharp.DataModels.Response.Raw;

namespace FinnHubSharp.DataModels.Response.FinnHub
{
    public class FinnHubListedSymbols : FinnHubResponseBase
    {
        public IEnumerable<ListedSymbol> ListedSymbols { get; set; }
    }
}