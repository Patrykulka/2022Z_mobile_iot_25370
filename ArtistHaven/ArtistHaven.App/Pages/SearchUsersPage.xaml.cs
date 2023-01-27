using ArtistHaven.Shared.DTOs;

namespace ArtistHaven.App.Pages;

public partial class SearchUsersPage : ContentPage
{
	public SearchUsersPage(ViewModels.UserSearchViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

    async private void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
		var curItem = e.SelectedItem as PublicUserDTO;
		await Shell.Current.GoToAsync($"User?UserName={curItem.UserName}");
    }
}