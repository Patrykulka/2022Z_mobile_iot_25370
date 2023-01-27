using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;

namespace ArtistHaven.App.Pages;

public partial class CurrentUserPosts : ContentPage {
	private UserManager _userManager;
    public CurrentUserPosts(UserManager userManager) {
        InitializeComponent();
        _userManager = userManager;
        this.Appearing += async (a, b) => {
            PrivateUserDTO user = await _userManager.Get();
            PostListView.ItemsSource = await _userManager.GetPost(user.UserName);
        };
    }
}