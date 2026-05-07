using System.Net;
using FinnHubSharp.Implementations;
using FinnHubSharp.Models.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Shouldly;

namespace FinnHubSharp.Tests;

[TestFixture]
public class FinnHubClientTests
{
    private const string ApiKey = "test-api-key";
    private const string BaseUrl = "https://finnhub.test/api/v1";

    [Test]
    public void Constructor_WhenApiKeyIsNull_ThrowsArgumentNullException()
    {
        var configuration = new FinnHubSharpConfiguration
        {
            BaseUrl = BaseUrl,
            ApiKey = null!
        };

        var exception = Should.Throw<ArgumentNullException>(() => new FinnHubClient(new HttpClient(), configuration));

        exception.ParamName.ShouldBe("ApiKey");
    }

    [Test]
    public void Constructor_WhenConfigurationIsNull_ThrowsArgumentNullException()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new FinnHubClient(new HttpClient(), null!));

        exception.ParamName.ShouldBe("configuration");
    }

    [Test]
    public void Constructor_WhenBaseUrlIsNull_ThrowsArgumentNullException()
    {
        var configuration = new FinnHubSharpConfiguration
        {
            BaseUrl = null!,
            ApiKey = ApiKey
        };

        var exception = Should.Throw<ArgumentNullException>(() => new FinnHubClient(new HttpClient(), configuration));

        exception.ParamName.ShouldBe("BaseUrl");
    }

    [Test]
    public async Task GetQuoteAsync_WhenResponseIsSuccessful_RequestsQuoteEndpointAndMapsResponse()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/quote?symbol=AAPL&token={ApiKey}")
            .Respond("application/json", """
            {
              "c": 181.25,
              "h": 183.5,
              "l": 179.4,
              "o": 180.0,
              "pc": 178.75,
              "t": 1715971200
            }
            """);
        var client = CreateClient(mockHttp);

        var result = await client.GetQuoteAsync("AAPL");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.OK);
        result.ErrorMessage.ShouldBeNull();
        result.Quote.ShouldNotBeNull();
        result.Quote.CurrentPrice.ShouldBe(181.25);
        result.Quote.High.ShouldBe(183.5);
        result.Quote.Low.ShouldBe(179.4);
        result.Quote.Open.ShouldBe(180.0);
        result.Quote.PreviousClose.ShouldBe(178.75);
        result.Quote.Timestamp.ShouldBe(1715971200);
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetQuoteAsync_WhenResponseIsUnsuccessful_SetsStatusCodeAndRawErrorMessage()
    {
        const string errorBody = """{"error":"Invalid API key"}""";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/quote?symbol=MSFT&token={ApiKey}")
            .Respond(HttpStatusCode.Unauthorized, "application/json", errorBody);
        var client = CreateClient(mockHttp);

        var result = await client.GetQuoteAsync("MSFT");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.Unauthorized);
        result.ErrorMessage.ShouldBe(errorBody);
        result.Quote.ShouldBeNull();
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetQuoteAsync_WhenResponseJsonIsInvalid_CapturesExceptionMessage()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/quote?symbol=BROKEN&token={ApiKey}")
            .Respond("application/json", "{ invalid json");
        var client = CreateClient(mockHttp);

        var result = await client.GetQuoteAsync("BROKEN");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.OK);
        result.Quote.ShouldBeNull();
        result.ErrorMessage.ShouldContain("System.Text.Json.JsonException");
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetSymbolInfoAsync_WhenResponseIsSuccessful_RequestsSearchEndpointAndMapsResponse()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/search?q=Apple&token={ApiKey}")
            .Respond("application/json", """
            {
              "count": 2,
              "result": [
                {
                  "description": "Apple Inc",
                  "displaySymbol": "AAPL",
                  "symbol": "AAPL",
                  "type": "Common Stock"
                },
                {
                  "description": "Apple Inc BDR",
                  "displaySymbol": "AAPL34.SA",
                  "symbol": "AAPL34.SA",
                  "type": "DR"
                }
              ]
            }
            """);
        var client = CreateClient(mockHttp);

        var result = await client.GetSymbolInfoAsync("Apple");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.OK);
        result.ErrorMessage.ShouldBeNull();
        result.SymbolInfo.ShouldNotBeNull();
        result.SymbolInfo.Count.ShouldBe(2);
        result.SymbolInfo.Result.Where(x => x.Symbol == "AAPL").ShouldHaveSingleItem();
        result.SymbolInfo.Result[0].Description.ShouldBe("Apple Inc");
        result.SymbolInfo.Result[0].DisplaySymbol.ShouldBe("AAPL");
        result.SymbolInfo.Result[0].Type.ShouldBe("Common Stock");
        result.SymbolInfo.Result[1].Symbol.ShouldBe("AAPL34.SA");
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetSymbolInfoAsync_WhenResponseIsUnsuccessful_SetsStatusCodeAndRawErrorMessage()
    {
        const string errorBody = """{"error":"Too many requests"}""";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/search?q=Apple&token={ApiKey}")
            .Respond(HttpStatusCode.TooManyRequests, "application/json", errorBody);
        var client = CreateClient(mockHttp);

        var result = await client.GetSymbolInfoAsync("Apple");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.TooManyRequests);
        result.ErrorMessage.ShouldBe(errorBody);
        result.SymbolInfo.ShouldBeNull();
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetAllSymbolsAsync_WhenResponseIsSuccessful_RequestsStockSymbolEndpointAndMapsResponse()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/stock/symbol?exchange=US&token={ApiKey}")
            .Respond("application/json", """
            [
              {
                "currency": "USD",
                "description": "Apple Inc",
                "displaySymbol": "AAPL",
                "figi": "BBG000B9XRY4",
                "mic": "XNAS",
                "symbol": "AAPL",
                "type": "Common Stock"
              },
              {
                "currency": "USD",
                "description": "Microsoft Corporation",
                "displaySymbol": "MSFT",
                "figi": "BBG000BPH459",
                "mic": "XNAS",
                "symbol": "MSFT",
                "type": "Common Stock"
              }
            ]
            """);
        var client = CreateClient(mockHttp);

        var result = await client.GetAllSymbolsAsync("US");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.OK);
        result.ErrorMessage.ShouldBeNull();
        var symbols = result.ListedSymbols.ToList();
        symbols.Count.ShouldBe(2);
        symbols[0].Currency.ShouldBe("USD");
        symbols[0].Description.ShouldBe("Apple Inc");
        symbols[0].DisplaySymbol.ShouldBe("AAPL");
        symbols[0].Figi.ShouldBe("BBG000B9XRY4");
        symbols[0].Mic.ShouldBe("XNAS");
        symbols[0].Symbol.ShouldBe("AAPL");
        symbols[0].Type.ShouldBe("Common Stock");
        symbols[1].Symbol.ShouldBe("MSFT");
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetAllSymbolsAsync_WhenResponseIsUnsuccessful_SetsStatusCodeAndRawErrorMessage()
    {
        const string errorBody = """{"error":"Exchange not found"}""";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/stock/symbol?exchange=NOPE&token={ApiKey}")
            .Respond(HttpStatusCode.BadRequest, "application/json", errorBody);
        var client = CreateClient(mockHttp);

        var result = await client.GetAllSymbolsAsync("NOPE");

        result.ResponseCode.ShouldBe((int)HttpStatusCode.BadRequest);
        result.ErrorMessage.ShouldBe(errorBody);
        result.ListedSymbols.ShouldBeNull();
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task GetAllSymbolsAsync_WhenHttpClientThrows_CapturesExceptionMessage()
    {
        var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Get &&
                    request.RequestUri == new Uri($"{BaseUrl}/stock/symbol?exchange=US&token={ApiKey}")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("network unavailable"));
        var client = CreateClient(handler.Object);

        var result = await client.GetAllSymbolsAsync("US");

        result.ResponseCode.ShouldBe(0);
        result.ListedSymbols.ShouldBeNull();
        result.ErrorMessage.ShouldContain("network unavailable");
        handler.VerifyAll();
    }

    private static FinnHubClient CreateClient(HttpMessageHandler handler) =>
        new(new HttpClient(handler), new FinnHubSharpConfiguration
        {
            BaseUrl = BaseUrl,
            ApiKey = ApiKey
        });
}
