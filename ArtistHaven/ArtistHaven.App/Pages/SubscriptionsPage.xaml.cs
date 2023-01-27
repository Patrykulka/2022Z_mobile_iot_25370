using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;
using System.Diagnostics;

namespace ArtistHaven.App.Pages;

public partial class SubscriptionsPage : ContentPage {
	private readonly UserManager _userManager;
	public SubscriptionsPage(UserManager userManager) {
		InitializeComponent();
		_userManager = userManager;
		this.Appearing += async (a, b) => {
			var l = await userManager.GetSubscriptions();
			if (l == null)
				Debug.WriteLine("NULL");
            SubscriptionList.ItemsSource = l;
		};
	}

    async private void SubscriptionList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
		if (e.SelectedItem == null)
			return;

		var selected = e.SelectedItem as SubscriptionDTO;
		await Shell.Current.GoToAsync($"UserPosts?UserName={selected.UserName}");
    }
}