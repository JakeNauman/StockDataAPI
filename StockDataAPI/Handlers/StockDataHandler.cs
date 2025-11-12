using StockDataAPI.Models;
using System.Text.Json.Nodes;

namespace StockDataAPI.Handlers
{
    public class StockDataHandler
    {
        private string _apiKey;
        private HttpClient _httpClient;

        public StockDataHandler(IConfiguration configuration, HttpClient httpClient)
        {
            // Handles API key secret configuration
            _apiKey = configuration["AlphaVantage:ApiKey"]!;
            if (_apiKey == null)
            {
                throw new ArgumentNullException("AlphaVantage:ApiKey");
            }
            _httpClient = httpClient;
        }

        // Queries external API to retrieve stock data by input string StockName
        // Groups by day on the past 30 days and calculates average High, Low and total Volume
        // Returns a list of StockData objects for each day
        public async Task<IEnumerable<StockData>> QueryDataAsync(string stockName)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={stockName}&interval=15min&outputsize=full&apikey={_apiKey}";
            Uri queryUri = new Uri(QUERY_URL);

            // Send query
            var response = await _httpClient.GetAsync(queryUri);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"API request failed with status code {response.StatusCode}");

            // Make sure data is found, and parse
            string jsonData = await response.Content.ReadAsStringAsync();
            var root = JsonNode.Parse(jsonData)!;
            var series = root["Time Series (15min)"]?.AsObject();
            if (series == null)
            {
                throw new ArgumentException($"Stock name not found: {stockName}");
            }


            // Convert to C# obj
            var points = series.Select(p => new
            {
                Timestamp = DateTime.Parse(p.Key),
                High = double.Parse(p.Value!["2. high"]!.ToString()),
                Low = double.Parse(p.Value!["3. low"]!.ToString()),
                Volume = double.Parse(p.Value!["5. volume"]!.ToString())
            }).ToList();

            // Group by day, perform calculations, convert to list of StockData objects
            var pointsByDay = points.GroupBy(p => p.Timestamp.Date)
                        .Select(t => new StockData
                        {
                            Day = DateOnly.FromDateTime(t.Key),
                            HighAverage = Math.Round(t.Average(x => x.High),4),
                            LowAverage = Math.Round(t.Average(x => x.Low),4),
                            Volume = t.Sum(x => (int)x.Volume),

                        }).ToList();

            return pointsByDay;
        }


    }
}



