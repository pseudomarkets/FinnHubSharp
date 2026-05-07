using FinnHubSharp.Models.Response.Raw;

namespace FinnHubSharp.Models.Response.FinnHub
{
    public class FinnHubSymbolInfo : FinnHubResponseBase
    {
        public SymbolInfo SymbolInfo { get; set; }
    }
}