using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Response.Raw
{
    public class ListedSymbol
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("displaySymbol")]
        public string DisplaySymbol { get; set; }

        [JsonPropertyName("figi")]
        public string Figi { get; set; }

        [JsonPropertyName("mic")]
        public string Mic { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
