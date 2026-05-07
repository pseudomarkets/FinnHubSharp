using System.Collections.Generic;
using System.Threading.Tasks;
using FinnHubSharp.Models.Request;
using FinnHubSharp.Models.Response.Raw;

namespace FinnHubSharp.Interfaces
{
    public interface IFinnHubStreamerClient
    {
        IAsyncEnumerable<IAsyncEnumerable<StreamingQuote>> GetStreamingQuotes(params StreamingSubscription[] subscriptions);

        Task Disconnect();
    }
}