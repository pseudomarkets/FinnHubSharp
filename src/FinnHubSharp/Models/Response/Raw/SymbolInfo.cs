using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Response.Raw
{
    public class SymbolInfo
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("result")]
        public List<Result> Result { get; set; }
    }

    public partial class Result
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("displaySymbol")]
        public string DisplaySymbol { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
