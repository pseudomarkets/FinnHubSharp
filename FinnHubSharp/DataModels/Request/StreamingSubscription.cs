using Newtonsoft.Json;

namespace FinnHubSharp.DataModels.Request
{
    public class StreamingSubscription
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}