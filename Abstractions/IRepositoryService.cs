using TestStocks.APIModels;

namespace TestStocks.Abstractions
{
    public interface IRepositoryService
    {
        Task<OperationResult<bool>> SaveStocksAsync(StocksResponse stocks);
    }
}
