using TestStocks.APIModels;

namespace TestStocks
{
    public static class UrlHelper
    {
        private static UriBuilder uriBuilder = new UriBuilder(Constants.BaseUrl);

        public static Uri CreateUriAggregates(string stockSymbol, DateTime from, DateTime to, TimeDuration time)
        {
            uriBuilder.Path = $"/v2/aggs/ticker/{stockSymbol}/range/1/{time}/{from:yyyy-MM-dd}/{to:yyyy-MM-dd}";

            return uriBuilder.Uri;
        }
    }
}
