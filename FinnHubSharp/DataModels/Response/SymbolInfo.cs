using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinnHubSharp.DataModels.Response
{
    public class SymbolInfo
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("displaySymbol")]
        public string DisplaySymbol { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}