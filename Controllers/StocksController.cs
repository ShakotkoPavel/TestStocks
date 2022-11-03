using Microsoft.AspNetCore.Mvc;
using TestStocks.Abstractions;
using TestStocks.APIModels;

namespace TestStocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksService _stocksService;

        public StocksController(IStocksService stocksService)
        {
            _stocksService = stocksService;
        }

        [HttpGet]
        public async Task<ActionResult> LoadStoks(string stockSymbol, TimeDuration timespan)
        {
            var response = await _stocksService.LoadStocksAsync(stockSymbol, timespan);

            if (response.Success)
            {
                return Ok(response.Result);
            }

            return BadRequest(response.Error);
        }
    }
}
