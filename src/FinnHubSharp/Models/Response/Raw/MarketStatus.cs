using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Response.Raw
{
    public class MarketStatus
    {
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("holiday")]
        public string Holiday { get; set; }

        [JsonPropertyName("isOpen")]
        public bool IsOpen { get; set; }

        [JsonPropertyName("session")]
        public string Session { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("sessionOpen")]
        public string SessionOpen { get; set; }

        [JsonPropertyName("sessionClose")]
        public string SessionClose { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }
    }
}
