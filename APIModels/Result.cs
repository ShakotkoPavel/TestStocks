using System.Text.Json.Serialization;

namespace TestStocks.APIModels
{
    public class Result
    {
        [JsonPropertyName("v")]
        public double TradingVolume { get; set; }

        [JsonPropertyName("vw")]
        public double VolumeAvarage { get; set; }

        [JsonPropertyName("o")]
        public double OpenPrice { get; set; }

        [JsonPropertyName("c")]
        public double ClosePrice { get; set; }

        [JsonPropertyName("h")]
        public double HighestPrice { get; set; }

        [JsonPropertyName("l")]
        public double LowestPrice { get; set; }

        [JsonPropertyName("t")]
        public long TimeStamp { get; set; }

        [JsonPropertyName("n")]
        public double NumberOfTransaction { get; set; }
    }
}
