using ArtistHaven.App.Services;
using ArtistHaven.Shared;
using ArtistHaven.Shared.Responses;

namespace ArtistHaven.App.Data {
    public class AuthManager {
        private IHttpClientService _httpClientService;

        public AuthManager(IHttpClientService httpClientService) {
            _httpClientService = httpClientService;
        }

        async public Task<AuthResponse> Login(LoginModel model) {
            var response = await _httpClientService.Post<AuthResponse>("Auth/Login", model);
            if (response.IsSuccessfull)
                _httpClientService.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", response.Message);
            return response;
        }

        async public Task<AuthResponse> Register(RegisterModel model) {
            return await _httpClientService.Post<AuthResponse>("Auth/Register", model);
        }
    }
}
