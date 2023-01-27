using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;
using ArtistHaven.Shared.Responses;
using System.Diagnostics;

namespace ArtistHaven.App.Pages;

[QueryProperty(nameof(SubscriptionTierName), "SubscriptionTierName")]
[QueryProperty(nameof(UserName), "UserName")]
public partial class SubscriptionTierPage : ContentPage {
	private readonly UserManager _userManager;

	private string userName;
	public string UserName {
		get => userName;
		set { 
			userName = value;
			UpdateUI();
		} 
	}

	private string subscriptionTierName;
	public string SubscriptionTierName {
		get => subscriptionTierName;
		set { 
			subscriptionTierName = value;
			UpdateUI();
		} 
	}

	public SubscriptionTierPage(UserManager userManager) {
		InitializeComponent();
		_userManager = userManager;
	}

	async void UpdateUI() {
		if (subscriptionTierName == null || userName == null)
			return;
		

		List<SubscriptionTierDTO> subTiers = await _userManager.GetSubscriptionTiers(userName);
		foreach(SubscriptionTierDTO subTier in subTiers) {
            Debug.WriteLine(subTier.Name);
            if (subTier.Name == subscriptionTierName) {
				BindingContext = subTier;
				return;
			}
		}
	}

    async private void SaveButtonClicked(object sender, EventArgs e) {
		string name = NameEntry.Text;
		string desc = DescriptionEditor.Text;
		decimal price = 0;
		int power = 0;

		if (!decimal.TryParse(PriceEntry.Text, out price)) {
			await DisplayAlert("Error", "Price has to be in decimal format", "Ok");
			return;
		}
		if (!int.TryParse(PowerEntry.Text, out power)) {
            await DisplayAlert("Error", "Power has to be an integer", "Ok");
            return;
		}
		if (name == string.Empty) {
            await DisplayAlert("Error", "Name can't be empty", "Ok");
            return;
		}
		if (desc == string.Empty) {
            await DisplayAlert("Error", "Description can't be empty", "Ok");
            return;
		}

		BasicResponse response;
        // create new subscriptionTier
        if (userName == null)
			response = await _userManager.CreateSubTier(new Shared.CreateSubscriptionTierModel() {
				Name = name,
				Description = desc,
				Price = price,
				SubscriptionPower = power
			});
        // update existing sub tier
        else
            response = await _userManager.EditSubTier(power, new Shared.Models.EditSubscriptionTierModel() {
				Name = name,
				Description = desc,
				Price = price
			});

		if (response == null)
			await DisplayAlert("Error", "Couldn't get response", "Ok");
		else if (!response.IsSuccessfull)
			await DisplayAlert("Error", response.Message, "Ok");

		await Shell.Current.GoToAsync("..");
    }
}