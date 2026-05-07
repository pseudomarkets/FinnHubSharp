using FinnHubSharp.DataModels.Response.Raw;

namespace FinnHubSharp.DataModels.Response.FinnHub
{
    public class FinnHubQuote : FinnHubResponseBase
    {
        public Quote Quote { get; set; }
    }
}