using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;
using System.Diagnostics;
namespace ArtistHaven.App.Pages;

public partial class AddPostPage : ContentPage {
    private readonly UploadManager _uploadManager;
    private readonly UserManager userManager;
    private Stream stream = new MemoryStream();
    private List<SubscriptionTierDTO> subscriptionTiers;
    private SubscriptionTierDTO subscriptionTier;

    public AddPostPage(UploadManager uploadManager, UserManager userManager) {
        InitializeComponent();
        _uploadManager = uploadManager;
        this.userManager = userManager;
        this.Appearing += async (a, b) => {
            PrivateUserDTO privateUser = await userManager.Get();
            subscriptionTiers = await userManager.GetSubscriptionTiers(privateUser.UserName);
        };
    }

    async private void AddMediaClicked(object sender, EventArgs e) {
        if (MediaPicker.Default.IsCaptureSupported) {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null) {
                this.stream = await photo.OpenReadAsync();
                Stream stream = new MemoryStream();
                this.stream.CopyTo(stream);
                this.stream.Position = 0;
                stream.Position = 0;
                var image = ImageSource.FromStream(() => stream);
                PickedImage.Source = image;
            }
        }
    }

    async private void AddPostButtonClick(object sender, EventArgs e) {
        if (subscriptionTier == null) {
            await DisplayAlert("Error", "Choose a subscription tier", "Ok");
            return;
        }

        var response = await _uploadManager.Upload(stream);
        if (response == null) {
            await DisplayAlert("Error", "No response", "Ok");
            return;
        }

        int mediaId = response.MediaID;
        var r = await userManager.CreatePost(new Shared.Models.CreatePostModel() {
            MediaId = mediaId,
            Text = TextEditor.Text,
            RequiredSubscriptionPower = subscriptionTier.Power
        });
        if (r == null) {
            await DisplayAlert("Error", "No response from post creation", "Ok");
            return;
        }
            
        if (r.IsSuccessfull) {
            await DisplayAlert("Success", "Post uploaded successfully", "Ok");
            return;
        } else
            await DisplayAlert("Error", r.Message, "Ok");
    }

    private async void SelectSubscriptionTierButtonClicked(object sender, EventArgs e) {
        if (subscriptionTiers == null)
            return;

        string result = await DisplayActionSheet("Subcription Tiers", "Cancel", null, subscriptionTiers.Select(sub => sub.Name).ToArray());
        foreach (var subTier in subscriptionTiers)
            if (subTier.Name == result)
                subscriptionTier = subTier;
    }
}