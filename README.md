# FinnHubSharp

FinnHubSharp is a .NET Standard client for the [Finnhub](https://finnhub.io/) finance APIs. It supports common REST endpoints and streaming trade data over WebSockets.

## Requirements

- .NET SDK 10.0 or later for building this repository
- Library target: `netstandard2.1`
- Test target: `net10.0`

## Repository Layout

```text
src/
  FinnHubSharp/              # Client library
tests/
  FinnHubSharp.Tests/        # NUnit unit tests
utilities/
  FinnHubSharpConsole/       # Console runner/sample app
```

The solution includes `src`, `tests`, and `utilities` as solution folders for IDEs such as Visual Studio and Rider.

## Install

```bash
dotnet add package FinnHubSharp
```

## Usage

### REST Client

```csharp
using FinnHubSharp.Implementations;
using FinnHubSharp.Models.Configuration;

var httpClient = new HttpClient();
var client = new FinnHubClient(
    httpClient,
    new FinnHubSharpConfiguration
    {
        ApiKey = "YOUR_API_KEY"
    });

var quote = await client.GetQuoteAsync("AAPL");

if (quote.ErrorMessage is null)
{
    Console.WriteLine($"Current price for AAPL: {quote.Quote.CurrentPrice}");
}
else
{
    Console.WriteLine($"Finnhub request failed: {quote.ErrorMessage}");
}
```

Available REST methods:

- `GetQuoteAsync(string symbol)`
- `GetSymbolInfoAsync(string symbolOrSecurityName)`
- `GetAllSymbolsAsync(string exchange)`

### Streaming Client

```csharp
using System.Net.WebSockets;
using FinnHubSharp.Implementations;
using FinnHubSharp.Models.Request;

var streamerClient = new FinnHubStreamerClient(new ClientWebSocket(), "YOUR_API_KEY");

await foreach (var stream in streamerClient.GetStreamingQuotes(
    new StreamingSubscription
    {
        Type = "subscribe",
        Symbol = "BINANCE:BTCUSDT"
    }))
{
    await foreach (var message in stream)
    {
        foreach (var trade in message.Data)
        {
            Console.WriteLine($"{trade.Symbol}: {trade.Price}");
        }
    }
}
```

## JSON

FinnHubSharp uses `System.Text.Json` for JSON serialization and deserialization. The raw response models use `JsonPropertyName` attributes to preserve Finnhub wire names such as `c`, `pc`, `displaySymbol`, and `type`.

## Development

Restore and build:

```bash
dotnet restore FinnHubSharp.sln
dotnet build FinnHubSharp.sln --no-restore
```

Run tests:

```bash
dotnet test tests/FinnHubSharp.Tests/FinnHubSharp.Tests.csproj --no-restore
```

The test suite uses NUnit, Moq, RichardSzalay.MockHttp, and Shouldly.

Run the console sample:

```bash
dotnet run --project utilities/FinnHubSharpConsole/FinnHubSharpConsole.csproj
```

## License

MIT
