using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;

namespace SimpleTrader.API.Services
{
    public class MajorIndexService : IMajorIndexService
    {
        private readonly FinancialModelingPropHttpClientFactory _httpClientFactory;

        public MajorIndexService(FinancialModelingPropHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MajorIndex> GetMajorIndex(MajorIndexType indexType)
        {
            using (FinancialModelingPrepHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string url = "majors-indexes/" + GetUriSuffix(indexType);

                MajorIndex majorIndex = await client.GetAsync<MajorIndex>(url);
                majorIndex.Type = indexType;

                return majorIndex;
            }
        }

        private string GetUriSuffix(MajorIndexType indexType)
        {
            switch (indexType)
            {
                case MajorIndexType.DowJones:
                    return ".DJI";
                case MajorIndexType.Nasdaq:
                    return ".XIC";
                case MajorIndexType.SP500:
                    return ".INX";
                default:
                    throw new Exception($"{nameof(MajorIndexType)} does not have a suffix defined.");
            }
        }
    }
}
