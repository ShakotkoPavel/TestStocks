namespace TestStocks.APIModels
{
    public class PerfomanceDataResponse
    {
        public string Ticker { get; set; }
        public IEnumerable<PerfomanceResult> PerfomanceResults { get; set; }
    }
}
