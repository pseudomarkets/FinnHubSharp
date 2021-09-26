using Newtonsoft.Json;

namespace FinnHubSharp.DataModels.Response
{
    public class ListedSymbol
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("displaySymbol")]
        public string DisplaySymbol { get; set; }

        [JsonProperty("figi")]
        public string Figi { get; set; }

        [JsonProperty("mic")]
        public string Mic { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}