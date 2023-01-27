namespace ArtistHaven.Shared.DTOs {
    public class PrivateUserDTO : PublicUserDTO {
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
