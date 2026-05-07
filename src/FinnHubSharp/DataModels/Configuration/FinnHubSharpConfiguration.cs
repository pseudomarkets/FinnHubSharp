namespace FinnHubSharp.DataModels.Configuration
{
    public class FinnHubSharpConfiguration
    {
        public string BaseUrl { get; set; } = "https://finnhub.io/api/v1";
        public string ApiKey { get; set; }
    }
}