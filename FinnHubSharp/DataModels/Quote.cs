using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinnHubSharp.DataModels
{
    public class Quote
    {
        [JsonProperty("c")]
        public double CurrentPrice { get; set; }

        [JsonProperty("h")]
        public double High { get; set; }

        [JsonProperty("l")]
        public double Low { get; set; }

        [JsonProperty("o")]
        public double Open { get; set; }

        [JsonProperty("pc")]
        public double PreviousClose { get; set; }

        [JsonProperty("t")]
        public long Timestamp { get; set; }
    }
}
