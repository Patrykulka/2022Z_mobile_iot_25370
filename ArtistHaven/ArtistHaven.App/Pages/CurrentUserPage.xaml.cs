using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ArtistHaven.App.Pages;

public partial class CurrentUserPage : ContentPage {
	private UserManager _userManager;
	private ObservableCollection<SubscriptionTierDTO> _subscriptionTierDTOs;
	private PrivateUserDTO _userDTO;
	public CurrentUserPage(UserManager userMananger) {
		InitializeComponent();
		_userManager = userMananger;
		this.Appearing += async (a, b) => {
			_userDTO = await userMananger.Get();
			BindingContext = _userDTO;
			_subscriptionTierDTOs = new ObservableCollection<SubscriptionTierDTO>(await userMananger.GetSubscriptionTiers(_userDTO.UserName));
            SubscriptionTierCarousel.ItemsSource = _subscriptionTierDTOs;
		};
	}

    private async void DeleteButtonClicked(object sender, EventArgs e) {
		SubscriptionTierDTO subTier = (SubscriptionTierDTO) SubscriptionTierCarousel.CurrentItem;
		bool answer = await DisplayAlert("Confirm", $"Do you want to delete {subTier.Name} subscription tier?", "Yes", "No");
		if (answer) {
			await _userManager.DeleteSubTier(subTier.Power);
            _subscriptionTierDTOs.Remove(subTier);
        }
    }

    private async void EditButtonClicked(object sender, EventArgs e) {
		await Shell.Current.GoToAsync($"SubscriptionTier" +
			$"?SubscriptionTierName={((SubscriptionTierDTO)SubscriptionTierCarousel.CurrentItem).Name}" +
			$"&UserName={((PrivateUserDTO)BindingContext).UserName}");
    }

	private async void AddButtonClicked(object sender, EventArgs e) {
		await Shell.Current.GoToAsync($"SubscriptionTier");
	}
}