using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.App.Services {
    static class TokenService {
        static public string GetToken() {
            return Preferences.Get("AuthToken", null);
        }

        static public void SetToken(string token) {
            Preferences.Set("AuthToken", token);
        }

        static public void RemoveToken() {
            Preferences.Remove("AuthToken");
        }
    }
}
