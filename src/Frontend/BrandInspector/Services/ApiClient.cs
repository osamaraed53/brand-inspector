using BrandInspector.Services;
using BrandInspector.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrandInspector.Services
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

    }
    public class ErrorResponse
    {

        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public string TraceId { get; set; }
    }



    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;


        public ApiClient(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;

        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    _tokenService.Token
                );

            var result = await _httpClient.GetAsync(url);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            return result;
        }

        public async Task<(bool, string)> LoginAsync(string username, string password)
        {

            var request = new { username, password };
            var json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync("/auth/login", content).ConfigureAwait(false))
            {
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return (false, JsonConvert.DeserializeObject<ErrorResponse>(responseJson).Detail);
                }

                _tokenService.Token = JsonConvert.DeserializeObject<LoginResponse>(responseJson).AccessToken;
                return (true, responseJson);
            }
        }
    }
}








