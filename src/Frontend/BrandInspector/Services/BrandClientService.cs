using BrandInspector.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrandInspector.Services
{
    public class BrandClientService : IBrandClientService
    {
        private readonly ApiClient _apiClient;
        public BrandClientService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IList<string>> GetBrandColors()
        {
           var response = await _apiClient.GetAsync("/brand/colors");
            return await Deserialize<string>(response);

        }

        public async Task<IList<string>> GetBrandFonts()
        {
            var response = await _apiClient.GetAsync("/brand/fonts");
            return await Deserialize<string>(response);
        }

        public async Task<IList<double>> GetBrandSizes()
        {
            var response =  await _apiClient.GetAsync("/brand/sizes");
            return await Deserialize<double>(response);

        }

        private async Task<List<T>> Deserialize<T>(HttpResponseMessage response)
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<T>>(responseJson);
        }


    }
}
