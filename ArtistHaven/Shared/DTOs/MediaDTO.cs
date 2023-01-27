using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArtistHaven.Shared.DTOs {
    public class MediaDTO {
        public string Name { get; set; }
        public string Data { get; set; }

        [JsonIgnore]
        public byte[] DataAsByteArray {
            get {
                Debug.WriteLine("Decrypting?");
                return Convert.FromBase64String(Data);
            } 
        }
    }
}
