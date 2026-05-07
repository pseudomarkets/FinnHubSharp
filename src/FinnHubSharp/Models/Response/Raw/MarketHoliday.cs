using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinnHubSharp.Models.Response.Raw
{
    public class MarketHoliday
    {
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("data")]
        public List<MarketHolidayData> Data { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
    }

    public class MarketHolidayData
    {
        [JsonPropertyName("eventName")]
        public string EventName { get; set; }

        [JsonPropertyName("atDate")]
        public string AtDate { get; set; }

        [JsonPropertyName("tradingHour")]
        public string TradingHour { get; set; }
    }
}
