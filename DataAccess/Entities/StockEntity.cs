namespace TestStocks.DataAccess.Entities
{
    public class StockEntity
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; }
        public int QueryCount { get; set; }
        public int ResultsCount { get; set; }
        public bool Adjusted { get; set; }
        public string RequestId { get; set; }
        public int Count { get; set; }
        public ICollection<ResultEntity> Results { get; set; }
    }
}
