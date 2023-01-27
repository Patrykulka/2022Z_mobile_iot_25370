using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.Shared.Models {
    public class EditSubscriptionTierModel {
        [Required]
        public string Name { get; set; }
        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
