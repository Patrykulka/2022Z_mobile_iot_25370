using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(Id))]
    public class Post {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RequiredSubscriptionPower { get; set; }
        public string? Text { get; set; }

        public virtual List<PostMedia> Media { get; set; } = new();
        public virtual List<PostTag> Tags { get; set; } = new();
        [ForeignKey(nameof(UserId)), DeleteBehavior(DeleteBehavior.NoAction)]
        [JsonIgnore]
        public virtual User User { get; set; }
        [ForeignKey($"{nameof(RequiredSubscriptionPower)}, {nameof(UserId)}"), DeleteBehavior(DeleteBehavior.NoAction)]
        [JsonIgnore]
        public virtual SubscriptionTier RequiredSubscriptionTier { get; set; }

        public void HideForNonSubscribers() {
            this.Text = string.Empty;
            this.Media = new List<PostMedia>();
        }
    }
}
