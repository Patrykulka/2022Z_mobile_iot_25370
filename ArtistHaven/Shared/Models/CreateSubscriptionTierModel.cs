using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtistHaven.Shared {
    public class CreateSubscriptionTierModel {
        [Required]
        public string Name { get; set; }
        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [Required]
        public int SubscriptionPower { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
