using FinnHubSharp.Models.Response.Raw;

namespace FinnHubSharp.Models.Response.FinnHub
{
    public class FinnHubMarketStatus : FinnHubResponseBase
    {
        public MarketStatus MarketStatus { get; set; }
    }
}
