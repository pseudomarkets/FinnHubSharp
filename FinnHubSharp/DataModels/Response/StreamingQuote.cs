using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinnHubSharp.DataModels.Response
{
    public class StreamingQuote
    {
        [JsonProperty("data")]
        public List<Data> Data { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }  
    }

    public class Data
    {
        [JsonProperty("p")]
        public double Price { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("t")]
        public long Timestamp { get; set; }

        [JsonProperty("v")]
        public double Volume { get; set; }
        
        [JsonProperty("c")]
        public string TradeCondition { get; set; }
    }
}