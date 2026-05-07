using System.Text.Json;
using FinnHubSharp.Models.Request;
using FinnHubSharp.Models.Response.Raw;
using NUnit.Framework;
using Shouldly;

namespace FinnHubSharp.Tests;

[TestFixture]
public class JsonSerializationTests
{
    [Test]
    public void StreamingSubscription_SerializesUsingFinnHubWireNames()
    {
        var subscription = new StreamingSubscription
        {
            Type = "subscribe",
            Symbol = "BINANCE:BTCUSDT"
        };

        var json = JsonSerializer.Serialize(subscription);

        json.ShouldBe("""{"type":"subscribe","symbol":"BINANCE:BTCUSDT"}""");
    }

    [Test]
    public void Quote_DeserializesUsingFinnHubWireNames()
    {
        var quote = JsonSerializer.Deserialize<Quote>("""
        {
          "c": 10.5,
          "h": 11.5,
          "l": 9.25,
          "o": 10,
          "pc": 9.75,
          "t": 1715971200
        }
        """);

        quote.ShouldNotBeNull();
        quote.CurrentPrice.ShouldBe(10.5);
        quote.High.ShouldBe(11.5);
        quote.Low.ShouldBe(9.25);
        quote.Open.ShouldBe(10);
        quote.PreviousClose.ShouldBe(9.75);
        quote.Timestamp.ShouldBe(1715971200);
    }

    [Test]
    public void ListedSymbol_DeserializesUsingFinnHubWireNames()
    {
        var listedSymbol = JsonSerializer.Deserialize<ListedSymbol>("""
        {
          "currency": "USD",
          "description": "Apple Inc",
          "displaySymbol": "AAPL",
          "figi": "BBG000B9XRY4",
          "mic": "XNAS",
          "symbol": "AAPL",
          "type": "Common Stock"
        }
        """);

        listedSymbol.ShouldNotBeNull();
        listedSymbol.Currency.ShouldBe("USD");
        listedSymbol.Description.ShouldBe("Apple Inc");
        listedSymbol.DisplaySymbol.ShouldBe("AAPL");
        listedSymbol.Figi.ShouldBe("BBG000B9XRY4");
        listedSymbol.Mic.ShouldBe("XNAS");
        listedSymbol.Symbol.ShouldBe("AAPL");
        listedSymbol.Type.ShouldBe("Common Stock");
    }

    [Test]
    public void SymbolInfo_DeserializesUsingFinnHubWireNames()
    {
        var symbolInfo = JsonSerializer.Deserialize<SymbolInfo>("""
        {
          "count": 1,
          "result": [
            {
              "description": "Apple Inc",
              "displaySymbol": "AAPL",
              "symbol": "AAPL",
              "type": "Common Stock"
            }
          ]
        }
        """);

        symbolInfo.ShouldNotBeNull();
        symbolInfo.Count.ShouldBe(1);
        var result = symbolInfo.Result.ShouldHaveSingleItem();
        result.Description.ShouldBe("Apple Inc");
        result.DisplaySymbol.ShouldBe("AAPL");
        result.Symbol.ShouldBe("AAPL");
        result.Type.ShouldBe("Common Stock");
    }

    [Test]
    public void StreamingQuote_DeserializesUsingFinnHubWireNames()
    {
        var streamingQuote = JsonSerializer.Deserialize<StreamingQuote>("""
        {
          "data": [
            {
              "p": 101.25,
              "s": "BINANCE:BTCUSDT",
              "t": 1715971200,
              "v": 0.5,
              "c": "regular"
            }
          ],
          "type": "trade"
        }
        """);

        streamingQuote.ShouldNotBeNull();
        streamingQuote.Type.ShouldBe("trade");
        var data = streamingQuote.Data.ShouldHaveSingleItem();
        data.Price.ShouldBe(101.25);
        data.Symbol.ShouldBe("BINANCE:BTCUSDT");
        data.Timestamp.ShouldBe(1715971200);
        data.Volume.ShouldBe(0.5);
        data.TradeCondition.ShouldBe("regular");
    }
}
