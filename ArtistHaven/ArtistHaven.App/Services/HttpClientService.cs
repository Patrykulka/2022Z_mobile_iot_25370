using ArtistHaven.App.Shells;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtistHaven.App.Services {
    public interface IHttpClientService {
        HttpClient HttpClient { get; }
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri);
        Task<T> Post<T>(string uri, object model);
        Task<T> PostForm<T>(string uri, HttpContent model);
        Task<T> Put<T>(string uri, object model);
        Task<T> Delete<T>(string uri);
    }

    public class HttpClientService : IHttpClientService {
        private string baseUri = "https://artisthaven.azurewebsites.net/Api/";
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public HttpClient HttpClient { get; set; }
        public HttpClientService() {
            this.HttpClient = new HttpClient();
        }

        private string GetUri(string uri) {
            return $"{baseUri}{uri}";
        }

        async public Task<T> Get<T>(string uri) {
            try {
                string res = await HttpClient.GetStringAsync(GetUri(uri));
                Debug.WriteLine(res);
                return JsonSerializer.Deserialize<T>(res, jsonSerializerOptions);
            } catch(HttpRequestException e) {
                Debug.WriteLine(e.ToString());
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    TokenService.RemoveToken();
                    App.Current.MainPage = new UnauthorizedAppShell();
                }
            }
            return default;
        }

        async public Task<T> Post<T>(string uri, object model) {
            var msg = new HttpRequestMessage(HttpMethod.Post, GetUri(uri));
            msg.Content = JsonContent.Create(model);

            try {
                var response = await HttpClient.SendAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    TokenService.RemoveToken();
                    App.Current.MainPage = new UnauthorizedAppShell();
                    return default;
                }

                var returnedJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(returnedJson);
                Debug.WriteLine(response.StatusCode);
                return JsonSerializer.Deserialize<T>(returnedJson, jsonSerializerOptions);
            } catch {
                return default;
            }
        }

        async public Task<T> Post<T>(string uri) {
            var msg = new HttpRequestMessage(HttpMethod.Post, GetUri(uri));

            try {
                var response = await HttpClient.SendAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    TokenService.RemoveToken();
                    App.Current.MainPage = new UnauthorizedAppShell();
                    return default;
                }

                var returnedJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(returnedJson);
                Debug.WriteLine(response.StatusCode);
                return JsonSerializer.Deserialize<T>(returnedJson, jsonSerializerOptions);
            } catch {
                return default;
            }
        }

        async public Task<T> PostForm<T>(string uri, HttpContent model) {
            try {
                var response = await HttpClient.PostAsync(GetUri(uri), model);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    TokenService.RemoveToken();
                    App.Current.MainPage = new UnauthorizedAppShell();
                    return default;
                }
                var returnedJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(returnedJson);
                return JsonSerializer.Deserialize<T>(returnedJson, jsonSerializerOptions);
            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return default;
            }
        }

        async public Task<T> Put<T>(string uri, object model) {
            var msg = new HttpRequestMessage(HttpMethod.Put, GetUri(uri));
            msg.Content = JsonContent.Create(model);

            try {
                var response = await HttpClient.SendAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    TokenService.RemoveToken();
                    App.Current.MainPage = new UnauthorizedAppShell();
                    return default;
                }

                var returnedJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(returnedJson);
                Debug.WriteLine(response.StatusCode);
                return JsonSerializer.Deserialize<T>(returnedJson, jsonSerializerOptions);
            } catch {
                return default;
            }
        }

        async public Task<T> Delete<T>(string uri) {
            var msg = new HttpRequestMessage(HttpMethod.Delete, GetUri(uri));

            try {
                var response = await HttpClient.SendAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    TokenService.RemoveToken();
                    App.Current.MainPage = new UnauthorizedAppShell();
                    return default;
                }

                var returnedJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(returnedJson);
                Debug.WriteLine(response.StatusCode);
                return JsonSerializer.Deserialize<T>(returnedJson, jsonSerializerOptions);
            } catch {
                return default;
            }
        }
    }
}
