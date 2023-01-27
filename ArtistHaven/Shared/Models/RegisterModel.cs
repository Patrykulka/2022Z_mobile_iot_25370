﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.Shared {
    public class RegisterModel {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(4)]
        public string Username { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required, MinLength(8)]
        public string ConfirmPassword { get; set; }
    }
}
