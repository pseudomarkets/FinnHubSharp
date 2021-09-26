using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinnHubSharp.DataModels.Request;
using FinnHubSharp.DataModels.Response;

namespace FinnHubSharp.Interfaces
{
    public interface IFinnHubStreamerClient
    {
        IAsyncEnumerable<IAsyncEnumerable<StreamingQuote>> GetStreamingQuotes(params StreamingSubscription[] subscriptions);

        Task Disconnect();
    }
}