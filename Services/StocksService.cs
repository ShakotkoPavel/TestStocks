using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using TestStocks.Abstractions;
using TestStocks.APIModels;

namespace TestStocks.Services
{
    public class StocksService : IStocksService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepositoryService _repositoryService;

        public StocksService(IHttpClientFactory httpClientFactory, IRepositoryService repositoryService)
        {
            _httpClientFactory = httpClientFactory;
            _repositoryService = repositoryService;
        }

        public async Task<OperationResult<Response>> LoadStocksAsync(string stockSymbol, TimeDuration timespan)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.ApiKey);

            DateTime mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
            DateTime sundayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);

            var stockProcessTask = ProcessStocksAsync(client, stockSymbol, mondayOfLastWeek, sundayOfLastWeek, timespan);
            var spyProcessTask = ProcessStocksAsync(client, Constants.SP500Index, mondayOfLastWeek, sundayOfLastWeek, timespan);

            var stockResponseResult = await stockProcessTask;
            var spyResponseResult = await spyProcessTask;

            if (!stockResponseResult.Success || !spyResponseResult.Success)
            {
                return OperationResult<Response>.FromError($"{stockResponseResult.Error} {spyResponseResult.Error}");
            }

            var perfomanceStockResult = Task.Run(() => CalculatePerfomance(stockResponseResult.Result));
            var perfomanceSpyResult = Task.Run(() => CalculatePerfomance(spyResponseResult.Result));

            var resultStock = await perfomanceStockResult;
            var resultSpy = await perfomanceSpyResult;

            if (!resultStock.Success || !resultSpy.Success)
            {
                return OperationResult<Response>.FromError("ERROR result is not success!!!");
            }

            var stockPerfomanceDataResponse = new PerfomanceDataResponse
            {
                Ticker = stockSymbol,
                PerfomanceResults = resultStock.Result
            };

            var spyPerfomanceDataResponse = new PerfomanceDataResponse
            {
                Ticker = Constants.SP500Index,
                PerfomanceResults = resultSpy.Result
            };

            var response = new Response { Results = new List<PerfomanceDataResponse> { stockPerfomanceDataResponse, spyPerfomanceDataResponse } };

            return OperationResult<Response>.Ok(response);
        }

        private async Task<OperationResult<StocksResponse>> ProcessStocksAsync(HttpClient httpClient, string stockSymbol, DateTime from, DateTime to, TimeDuration time)
        {
            var httpStockResponseMessage = await httpClient.GetAsync(UrlHelper.CreateUriAggregates(stockSymbol, from, to, time));

            if (httpStockResponseMessage.StatusCode is HttpStatusCode.OK)
            {
                var content = await httpStockResponseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<StocksResponse>(content);

                if (response is not null && response.Status is Status.OK)
                {
                    if (!string.Equals(stockSymbol, Constants.SP500Index))
                    {
                        var saveResult = await _repositoryService.SaveStocksAsync(response);

                        if (!saveResult.Success)
                        {
                            return OperationResult<StocksResponse>.FromError(saveResult.Error);
                        }
                    }

                    return OperationResult<StocksResponse>.Ok(response);
                }
            }

            return OperationResult<StocksResponse>.FromError("ProcessStocksAsync ERROR!!!!");
        }

        private OperationResult<List<PerfomanceResult>> CalculatePerfomance(StocksResponse stock)
        {
            var results = stock.Results.ToArray();

            var perfomanceResults = new List<PerfomanceResult>();
            var result = results.FirstOrDefault();

            if (result is not null)
            {
                var basePrice = result.VolumeAvarage;

                for (var i = 0; i < results.Length; i++)
                {
                    perfomanceResults.Add(new PerfomanceResult
                    {
                        Data = Math.Round((results[i].VolumeAvarage * 100 / basePrice) - 100, 3),
                        TimeStamp = results[i].TimeStamp
                    });
                }

                return OperationResult<List<PerfomanceResult>>.Ok(perfomanceResults);
            }

            return OperationResult<List<PerfomanceResult>>.FromError("ERROR Results is empty!!!");
        }
    }
}
