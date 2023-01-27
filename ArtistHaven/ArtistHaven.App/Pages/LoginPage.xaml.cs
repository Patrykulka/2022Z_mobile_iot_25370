using ArtistHaven.App.Data;
using ArtistHaven.App.Services;
using ArtistHaven.App.Shells;
using ArtistHaven.Shared;
using ArtistHaven.Shared.Responses;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ArtistHaven.App.Pages;

public partial class LoginPage : ContentPage {
	private AuthManager _authManager;
    public LoginPage(AuthManager authManager) {
        InitializeComponent();
        _authManager = authManager;
    }

    async private void LoginButtonClicked(object sender, EventArgs e) {
		string email = EMailEntry.Text;
		string password = PasswordEntry.Text;
        if (!new EmailAddressAttribute().IsValid(email)) {
            await DisplayAlert("Error", "Email is invalid", "Ok");
            return;
        }

        if (password.Equals(string.Empty)) {
            await DisplayAlert("Error", "Password can't be empty", "Ok");
            return;
        }

        SignInButton.IsEnabled = false;
        try {
            LoginModel model = new LoginModel(email, password);
            var response = await _authManager.Login(model);
            if (!response.IsSuccessfull) {
                await DisplayAlert("Error", response.ToString(), "Ok");
            } else {
                TokenService.SetToken(response.Message);
                App.Current.MainPage = new AuthorizedAppShell();
            }
        } catch (Exception err) {
            await DisplayAlert("Error", err.Message, "Ok");
        } finally {
            SignInButton.IsEnabled = true;
        }
    }
}