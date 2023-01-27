using ArtistHaven.App.Data;
using ArtistHaven.App.Services;
using ArtistHaven.Shared;
using System.ComponentModel.DataAnnotations;

namespace ArtistHaven.App.Pages;

public partial class RegisterPage : ContentPage{
	private AuthManager authManager;
	public RegisterPage(AuthManager manager) {
		InitializeComponent();
		authManager = manager;
	}

    async private void RegisterButtonClicked(object sender, EventArgs e) {
        string email = EMailEntry.Text;
        string password = PasswordEntry.Text;
        string userName = UserNameEntry.Text;
        
        if (!new EmailAddressAttribute().IsValid(email)) {
            await DisplayAlert("Error", "Email is invalid", "Ok");
            return;
        }

        if (password.Equals(string.Empty)) {
            await DisplayAlert("Error", "Password can't be empty", "Ok");
            return;
        }

        if (userName.Equals(string.Empty)) {
            await DisplayAlert("Error", "Username can't be empty", "Ok");
            return;
        }

        if (password != ConfirmPasswordEntry.Text) {
            await DisplayAlert("Error", "Passwords aren't the same", "Ok");
            return;
        }



        RegisterButton.IsEnabled = false;
        try {
            RegisterModel model = new RegisterModel() { 
                Username = userName,
                Email = email,
                Password = password,
                ConfirmPassword = password
            };
            var response = await authManager.Register(model);
            if (!response.IsSuccessfull) {
                await DisplayAlert("Error", response.ToString(), "Ok");
            } else {
                await DisplayAlert("Success", "Successfully registered!", "Ok");
            }
        } catch (Exception err) {
            await DisplayAlert("Error", err.Message, "Ok");
        } finally {
            RegisterButton.IsEnabled = true;
        }
    }
}