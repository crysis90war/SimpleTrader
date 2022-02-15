using Newtonsoft.Json;

namespace SimpleTrader.API
{
    public class FinancialModelingPrepHttpClient : HttpClient
    {
        private readonly string _apiKey;

        public FinancialModelingPrepHttpClient(string apiKey)
        {
            this.BaseAddress = new Uri("http://localhost:3000/");
            _apiKey = apiKey;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            //HttpResponseMessage response = await GetAsync($"{url}?apiKey={_apiKey}");
            HttpResponseMessage response = await GetAsync($"{url}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}
