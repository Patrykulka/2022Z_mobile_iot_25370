using ArtistHaven.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(SubscriptionPower), nameof(UserId))]
    [Index(nameof(Name), nameof(UserId), IsUnique = true)]
    public class SubscriptionTier {
        public int SubscriptionPower { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public SubscriptionTierDTO ToDTO() {
            return new SubscriptionTierDTO() {
                Name = Name,
                Price = Price,
                Description = Description,
                Power = SubscriptionPower
            };
        }
    }
}
