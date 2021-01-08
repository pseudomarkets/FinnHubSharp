# FinnHubSharp
.NET Standard client for accessing FinnHub.io finance APIs

# Requirements
* .NET Standard 2.0
* Newtonsoft.JSON

# Usage

`using FinnHubSharp;`

`FinnHubClient client = new FinnHubClient("YOUR_API_KEY");`

`var quote = await client.GetQuoteAsync("AAPL");`

`var price = quote.CurrentPrice;`


# NuGet
https://nuget.pseudomarkets.live/packages/FinnHubSharp 


