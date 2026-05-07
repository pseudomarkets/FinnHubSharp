using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Response.Raw
{
    public class StreamingQuote
    {
        [JsonPropertyName("data")]
        public List<Data> Data { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }  
    }

    public class Data
    {
        [JsonPropertyName("p")]
        public double Price { get; set; }

        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        [JsonPropertyName("v")]
        public double Volume { get; set; }
        
        [JsonPropertyName("c")]
        public string TradeCondition { get; set; }
    }
}
