using SimpleTrader.API.Results;
using SimpleTrader.Domain.Exceptions;
using SimpleTrader.Domain.Services;

namespace SimpleTrader.API.Services
{
    public class StockPriceService : IStockPriceService
    {
        private readonly FinancialModelingPropHttpClientFactory _httpClientFactory;

        public StockPriceService(FinancialModelingPropHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<double> GetPrice(string symbol)
        {
            using (FinancialModelingPrepHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string url = "real-time-price/" + symbol;

                StockPriceResult stockPriceResult = await client.GetAsync<StockPriceResult>(url);

                if (stockPriceResult.Price == 0)
                {
                    throw new InvalidSymbolException(symbol);
                }

                return stockPriceResult.Price;
            }
        }
    }
}
