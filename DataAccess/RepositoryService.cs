using TestStocks.Abstractions;
using TestStocks.APIModels;
using TestStocks.DataAccess.Entities;

namespace TestStocks.DataAccess
{
    public class RepositoryService : IRepositoryService
    {
        private readonly StocksDbContext _dbContext;

        public RepositoryService(StocksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<bool>> SaveStocksAsync(StocksResponse stocks)
        {
            var stockEntity = DbMapper.Map<StockEntity>(stocks);

            await _dbContext.Stocks.AddAsync(stockEntity);

            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult > 1)
            {
                return OperationResult<bool>.Ok(true);
            }

            return OperationResult<bool>.FromError("Error with database!!!");
        }
    }
}
