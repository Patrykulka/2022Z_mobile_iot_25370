using ArtistHaven.App.Services;
using ArtistHaven.App.Shells;

namespace ArtistHaven.App {
    public partial class App : Application {
        public App(IHttpClientService clientService) {
            InitializeComponent();
            if (TokenService.GetToken() != null) {
                MainPage = new AuthorizedAppShell();
                clientService.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenService.GetToken());
            } else
                MainPage = new UnauthorizedAppShell();
        }
    }
}