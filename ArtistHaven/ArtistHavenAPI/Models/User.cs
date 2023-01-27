using ArtistHaven.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class User: IdentityUser<int> {
        [JsonIgnore]
        public virtual List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        [JsonIgnore]
        public virtual List<Subscription> Subscribers { get; set; } = new List<Subscription>();
        [JsonIgnore]
        public virtual List<SubscriptionTier> SubscriptionTiers { get; set; } = new List<SubscriptionTier>();
        [JsonIgnore]
        public virtual List<Post> Posts { get; set; } = new List<Post>();

        public PublicUserDTO ToPublicDTO() {
            return new PublicUserDTO {
                UserName = UserName
            };
        }

        public PrivateUserDTO ToPrivateDTO() {
            return new PrivateUserDTO {
                UserName = UserName,
                Email = Email,
                EmailConfirmed = EmailConfirmed,
                PhoneNumber = PhoneNumber,
            };
        }
    }
}
