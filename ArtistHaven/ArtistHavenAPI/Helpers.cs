using ArtistHaven.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ArtistHaven.API {
    public class Helpers {
        public static int? GetCurrentUserId(HttpContext httpContext) {
            string? val = httpContext.User.Claims.FirstOrDefault(x => x != null && x.Type.Equals(ClaimTypes.NameIdentifier), null)?.Value;
            if (val == null)
                return null;

            if (int.TryParse(val, out int userId))
                return userId;
            return null;
        }

        public static Task<User> GetCurrentUserAsync(UserManager<User> userManager, HttpContext httpContext) {
            return userManager.GetUserAsync(httpContext.User);
        }
    }
}
