using ArtistHaven.App.Data;

namespace ArtistHaven.App.Pages;

[QueryProperty(nameof(UserName), "UserName")]
public partial class UserPosts : ContentPage
{
    private string userName;
    public string UserName {
        get => userName;
        set {
            userName = value;
            UpdateUI();
        }
    }

    private UserManager manager;
    public UserPosts(UserManager manager) {
		InitializeComponent();
        this.manager = manager;
	}

    async public void UpdateUI() {
        PostListView.ItemsSource = await manager.GetPost(userName);
    }
}