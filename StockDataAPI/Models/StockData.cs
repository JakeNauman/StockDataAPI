namespace StockDataAPI.Models
{
    public class StockData
    {
        public DateOnly Day { get; set; }

        public double LowAverage { get; set; }

        public double HighAverage { get; set; }

        public int Volume { get; set; }
    }
}
