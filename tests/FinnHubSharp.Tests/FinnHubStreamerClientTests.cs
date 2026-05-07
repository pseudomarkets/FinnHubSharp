using System.Net.WebSockets;
using FinnHubSharp.Implementations;
using NUnit.Framework;
using Shouldly;

namespace FinnHubSharp.Tests;

[TestFixture]
public class FinnHubStreamerClientTests
{
    [Test]
    public void Constructor_WhenApiKeyIsNull_ThrowsArgumentNullException()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new FinnHubStreamerClient(new ClientWebSocket(), null!));

        exception.ParamName.ShouldBe("apiKey");
    }

    [Test]
    public async Task Disconnect_WhenSocketHasNotConnected_CompletesWithoutClosing()
    {
        using var socket = new ClientWebSocket();
        var client = new FinnHubStreamerClient(socket, "test-api-key");

        await Should.NotThrowAsync(client.Disconnect);

        socket.State.ShouldBe(WebSocketState.None);
    }
}
