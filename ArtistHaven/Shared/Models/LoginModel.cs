using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.Shared {
    public class LoginModel {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }

        public LoginModel(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}
