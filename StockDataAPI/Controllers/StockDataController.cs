using Microsoft.AspNetCore.Mvc;
using StockDataAPI.Models;
using StockDataAPI.Handlers;

namespace StockDataAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockDataController : ControllerBase
    {

        private StockDataHandler _handler;

        private readonly ILogger<StockDataController> _logger;

        public StockDataController(ILogger<StockDataController> logger, StockDataHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        // Given string StockName, returns a list of StockData objects
        //  with average High and Low values, and total Volume on each day
        //  for the last month (30 days)
        [HttpGet("{stockName}")]
        public async Task<ActionResult<IEnumerable<StockData>>> GetStockDataByName(string stockName)
        {
            try
            {
                var data = await _handler.QueryDataAsync(stockName);
                return Ok(data);
            }
            catch (ArgumentException ex) // Stock not found
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) // Server error
            {
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }

        }
    }
}
