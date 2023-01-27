using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.Shared.Models {
    public class CreatePostModel {
        [Required]
        public int RequiredSubscriptionPower { get; set; }
        [Required]
        public string Text { get; set; }
        public List<string> Tags { get; set; } = new();
        public int MediaId { get; set; }
    }
}
