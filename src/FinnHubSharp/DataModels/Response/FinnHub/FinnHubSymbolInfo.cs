using FinnHubSharp.DataModels.Response.Raw;

namespace FinnHubSharp.DataModels.Response.FinnHub
{
    public class FinnHubSymbolInfo : FinnHubResponseBase
    {
        public SymbolInfo SymbolInfo { get; set; }
    }
}