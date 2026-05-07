using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Response.Raw
{
    public class Quote
    {
        [JsonPropertyName("c")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("h")]
        public double High { get; set; }

        [JsonPropertyName("l")]
        public double Low { get; set; }

        [JsonPropertyName("o")]
        public double Open { get; set; }

        [JsonPropertyName("pc")]
        public double PreviousClose { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }
    }
}
