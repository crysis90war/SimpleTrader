namespace SimpleTrader.API
{
    public class FinancialModelingPropHttpClientFactory
    {
        private readonly string _apiKey;

        public FinancialModelingPropHttpClientFactory(string apiKey)
        {
            _apiKey = apiKey;
        }

        public FinancialModelingPrepHttpClient CreateHttpClient()
        {
            return new FinancialModelingPrepHttpClient(_apiKey);
        }
    }
}
