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

    [Test]
    public void MarketStatus_DeserializesUsingFinnHubWireNames()
    {
        var marketStatus = JsonSerializer.Deserialize<MarketStatus>("""
        {
          "exchange": "US",
          "holiday": "",
          "isOpen": true,
          "session": "regular",
          "state": "open",
          "timezone": "America/New_York",
          "sessionOpen": "2026-05-06 09:30:00",
          "sessionClose": "2026-05-06 16:00:00",
          "t": 1778088600
        }
        """);

        marketStatus.ShouldNotBeNull();
        marketStatus.Exchange.ShouldBe("US");
        marketStatus.Holiday.ShouldBe("");
        marketStatus.IsOpen.ShouldBeTrue();
        marketStatus.Session.ShouldBe("regular");
        marketStatus.State.ShouldBe("open");
        marketStatus.Timezone.ShouldBe("America/New_York");
        marketStatus.SessionOpen.ShouldBe("2026-05-06 09:30:00");
        marketStatus.SessionClose.ShouldBe("2026-05-06 16:00:00");
        marketStatus.Timestamp.ShouldBe(1778088600);
    }

    [Test]
    public void MarketHoliday_DeserializesUsingFinnHubWireNames()
    {
        var marketHoliday = JsonSerializer.Deserialize<MarketHoliday>("""
        {
          "exchange": "US",
          "data": [
            {
              "eventName": "Memorial Day",
              "atDate": "2026-05-25",
              "tradingHour": ""
            }
          ],
          "timezone": "America/New_York"
        }
        """);

        marketHoliday.ShouldNotBeNull();
        marketHoliday.Exchange.ShouldBe("US");
        marketHoliday.Timezone.ShouldBe("America/New_York");
        var holiday = marketHoliday.Data.ShouldHaveSingleItem();
        holiday.EventName.ShouldBe("Memorial Day");
        holiday.AtDate.ShouldBe("2026-05-25");
        holiday.TradingHour.ShouldBe("");
    }
}
