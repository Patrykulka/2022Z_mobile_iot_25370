using ArtistHaven.App.Pages;

namespace ArtistHaven.App.Shells {
    public partial class AuthorizedAppShell : Shell {
        public AuthorizedAppShell() {
            InitializeComponent();
            Routing.RegisterRoute("SubscriptionTier", typeof(SubscriptionTierPage));
            Routing.RegisterRoute("UserPosts", typeof(UserPosts));
            Routing.RegisterRoute("User", typeof(UserDetails));
        }
    }
}