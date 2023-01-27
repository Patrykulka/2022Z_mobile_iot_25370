using ArtistHaven.App.Data;
using ArtistHaven.App.Pages;
using ArtistHaven.App.Services;
using ArtistHaven.App.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;

namespace ArtistHaven.App {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseMauiCommunityToolkit();
            builder.RegisterServices().RegisterPages().RegisterViewModels();
            return builder.Build();
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder) {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<UserDetails>();
            builder.Services.AddTransient<SubscriptionTierPage>();
            builder.Services.AddTransient<AddPostPage>();
            builder.Services.AddTransient<SubscriptionsPage>();
            builder.Services.AddTransient<UserPosts>();
            builder.Services.AddTransient<SearchUsersPage>();

            builder.Services.AddSingleton<CurrentUserPage>();
            builder.Services.AddSingleton<CurrentUserPosts>();
            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder) {
            builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
            builder.Services.AddSingleton<AuthManager>();
            builder.Services.AddSingleton<UserManager>();
            builder.Services.AddSingleton<UploadManager>();
            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder) {
            builder.Services.AddSingleton<UserSearchViewModel>();
            return builder;
        }
    }
}