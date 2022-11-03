namespace TestStocks.DataAccess.Entities
{
    public class ResultEntity
    {
        public Guid Id { get; set; }

        public Guid? StockId { get; set; }

        public StockEntity Stock { get; set; }

        public double TradingVolume { get; set; }

        public double VolumeAvarage { get; set; }

        public double OpenPrice { get; set; }

        public double ClosePrice { get; set; }

        public double HighestPrice { get; set; }

        public double LowestPrice { get; set; }

        public long TimeStamp { get; set; }

        public double NumberOfTransaction { get; set; }
    }
}
