using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Request
{
    public class StreamingSubscription
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}
