using FinnHubSharp.Models.Response.Raw;

namespace FinnHubSharp.Models.Response.FinnHub
{
    public class FinnHubQuote : FinnHubResponseBase
    {
        public Quote Quote { get; set; }
    }
}