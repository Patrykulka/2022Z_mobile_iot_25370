using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.Shared.Responses {
    public class BasicResponse {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccessfull { get; set; } = false;
    }
}
