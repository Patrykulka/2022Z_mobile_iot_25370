using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ArtistHaven.App.Pages;

[QueryProperty(nameof(UserName), "UserName")]
public partial class UserDetails : ContentPage {
    private string userName;
    public string UserName {
        get => userName;
        set {
            userName = value;
            UpdateUI();
        }
    }

    private UserManager _userManager;
    public UserDetails(UserManager userManager) {
        InitializeComponent();
        _userManager = userManager;
    }

    async public void UpdateUI() {
        UsernameLabel.Text = userName;
        List<SubscriptionTierDTO> _subscriptionTierDTOs = new List<SubscriptionTierDTO>(await _userManager.GetSubscriptionTiers(userName));
        SubscriptionTierCarousel.ItemsSource = _subscriptionTierDTOs;
    }

    async private void SubscribeButtonClicked(object sender, EventArgs e) {
        var curSubTier = SubscriptionTierCarousel.CurrentItem as SubscriptionTierDTO;
        var r = await _userManager.Subscribe(userName, curSubTier.Power);
        if (r == null) {
            await DisplayAlert("Error", "Couldn't connect to server.", "Ok");
            return;
        }
        if (!r.IsSuccessfull) {
            await DisplayAlert("Error", r.Message, "Ok");
            return;
        } else {
            await DisplayAlert("Success", $"You have subscribed to {userName}!", "Ok");
            return;
        }
    }
}