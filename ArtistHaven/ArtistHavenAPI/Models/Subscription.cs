using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(SubscriberId), nameof(CreatorId))]
    public class Subscription {
        public int SubscriberId { get; set; }
        public int CreatorId { get; set; }
        public int SubscriptionTierPower { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(SubscriberId)), InverseProperty("Subscriptions"), DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual User Subscriber { get; set; }
        [ForeignKey(nameof(CreatorId)), InverseProperty("Subscribers"), DeleteBehavior(DeleteBehavior.NoAction)]
        [JsonIgnore]
        public virtual User Creator { get; set; }
        [JsonIgnore]
        [ForeignKey($"{nameof(SubscriptionTierPower)}, {nameof(CreatorId)}"), DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SubscriptionTier SubscriptionTier { get; set; }
    }
}
