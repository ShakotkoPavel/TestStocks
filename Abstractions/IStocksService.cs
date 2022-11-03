using TestStocks.APIModels;

namespace TestStocks.Abstractions
{
    public interface IStocksService
    {
        Task<OperationResult<Response>> LoadStocksAsync(string stockSymbol, TimeDuration time);
    }
}
