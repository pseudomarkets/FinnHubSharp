using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FinnHubSharp.DataModels.Request;
using FinnHubSharp.DataModels.Response;
using FinnHubSharp.Interfaces;
using Newtonsoft.Json;

namespace FinnHubSharp.Implementations
{
    public class FinnHubStreamerClient : IFinnHubStreamerClient, IDisposable
    {
        private readonly string _apiKey;
        private readonly ClientWebSocket _webSocketClient;

        public FinnHubStreamerClient(ClientWebSocket webSocketClient, string apiKey)
        {
            _webSocketClient = webSocketClient ?? new ClientWebSocket();
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }
        
        public async IAsyncEnumerable<IAsyncEnumerable<StreamingQuote>> GetStreamingQuotes(
            params StreamingSubscription[] subscriptions)
        {
            while(true)
            {
                using (var socket = _webSocketClient)
                {
                    await socket.ConnectAsync(new Uri($"wss://ws.finnhub.io?token={_apiKey}"), CancellationToken.None);
                    foreach (var sub in subscriptions.ToList())
                    {
                        await SendSubscription(socket, JsonConvert.SerializeObject(sub));
                    }
                    yield return ReceiveStreamingData(socket);
                }
            }
        }

        public async Task Disconnect()
        {
            if (_webSocketClient is {State: WebSocketState.Open})
            {
                await _webSocketClient.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
        }

        private async Task SendSubscription(ClientWebSocket socket, string data) =>
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None);
        
        private async IAsyncEnumerable<StreamingQuote> ReceiveStreamingData(ClientWebSocket socket)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (true)
            {
                using (var memoryStream = new MemoryStream())
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        memoryStream.Write(buffer.Array ?? Array.Empty<byte>(), buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        await Disconnect();

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        yield return JsonConvert.DeserializeObject<StreamingQuote>(await reader.ReadToEndAsync());
                    }
                }
            } 
        }

        public async void Dispose()
        {
            await Disconnect();
            _webSocketClient?.Dispose();
        }
    }
}