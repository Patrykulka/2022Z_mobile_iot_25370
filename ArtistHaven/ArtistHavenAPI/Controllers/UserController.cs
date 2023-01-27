using ArtistHaven.API.Data;
using ArtistHaven.API.Models;
using ArtistHaven.API.Services;
using ArtistHaven.Shared.DTOs;
using ArtistHaven.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArtistHaven.API.Controllers {
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserManager<User> _userManager;
        private readonly ArtistHavenAPIContext _context;
        private readonly IImageService _imageService;

        public UserController(UserManager<User> userManager, ArtistHavenAPIContext context, IImageService imageService) {
            _userManager = userManager;
            _context = context;
            _imageService = imageService;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        async public Task<List<User>> Users() {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("[action]")]
        public List<string> Claims() {
            List<string> claimValues = new();
            foreach (Claim claim in HttpContext.User.Claims) {
                claimValues.Add(claim.Type);
                claimValues.Add(claim.Value);
            }
            return claimValues;
        }

        [HttpGet]
        async public Task<ActionResult<PublicUserDTO>> GetCurrentUser() {
            User? user = await Helpers.GetCurrentUserAsync(_userManager, HttpContext);
            return Ok(user.ToPrivateDTO());
        }

        [HttpGet("{userName}")]
        async public Task<ActionResult<PublicUserDTO>> GetUserByName(string userName) {
            User? user = await _userManager.FindByNameAsync(userName);
            return user == null ? NotFound() : Ok(user.ToPublicDTO());
        }

        [HttpGet("{userName}/Posts")]
        async public Task<ActionResult<List<PostDTO>>> GetUserPosts(string userName) {
            List<Post> posts = await _context.Post
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Media)
                .ThenInclude(pm => pm.Media)
                .Where(p => p.User.UserName.Equals(userName))
                .ToListAsync();

            if (posts.Count == 0)
                return Ok(new());

            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new List<PostDTO>());

            List<PostDTO> dtoPosts = new();
            foreach (Post post in posts) {
                PostDTO dtoPost = new PostDTO() {
                    CreatorName = post.User.UserName,
                    Text = post.Text ?? "",
                };

                if (userId.Value == posts[0].UserId) {
                    foreach (PostMedia postMedia in post.Media) {
                        dtoPost.Media.Add(new MediaDTO() {
                            Name = postMedia.Media.FileName,
                            Data = Convert.ToBase64String(await _imageService.Download(postMedia.Media.Path))
                        });
                    };
                    dtoPosts.Add(dtoPost);
                };
            }

            if (userId.Value == posts[0].UserId)
                return Ok(dtoPosts);

            Subscription? subscription = await _context.Subscription.Where(sub => sub.SubscriberId == userId.Value && sub.CreatorId == posts[0].UserId).FirstOrDefaultAsync();
            if (subscription == null)
                return Forbid();

            dtoPosts = new();
            foreach (Post post in posts) {
                if (post.RequiredSubscriptionPower > subscription.SubscriptionTierPower)
                    post.HideForNonSubscribers();

                PostDTO dtoPost = new PostDTO() {
                    CreatorName = post.User.UserName,
                    Text = post.Text ?? "",
                };

                foreach (PostMedia postMedia in post.Media) {
                    dtoPost.Media.Add(new MediaDTO() {
                        Name = postMedia.Media.FileName,
                        Data = Convert.ToBase64String(await _imageService.Download(postMedia.Media.Path))
                    });
                }
                dtoPosts.Add(dtoPost);
            }

            return Ok(dtoPosts);
        }

        [HttpGet("{userName}/[action]")]
        async public Task<List<SubscriptionTierDTO>> SubscriptionTiers(string userName) {
            List<SubscriptionTier> subscriptionTiers = await _context.SubscriptionTier.Include(st => st.User).Where(st => st.User.UserName.Equals(userName)).ToListAsync();
            return subscriptionTiers.Select(st => st.ToDTO()).ToList();
        }

        [HttpGet("[action]")]
        async public Task<ActionResult<List<SubscriptionDTO>>> Subscriptions() {
            int? userId = Helpers.GetCurrentUserId(HttpContext);

            if (!userId.HasValue)
                return BadRequest(new List<SubscriptionDTO>());
            var x = await _context.Subscription
                .Where(sub => sub.SubscriberId.Equals(userId.Value))
                .Include(sub => sub.Creator)
                .ToListAsync();

            List<SubscriptionDTO> subscriptions = new List<SubscriptionDTO>();
            foreach (var sub in x)
                subscriptions.Add(new SubscriptionDTO() {
                    UserName = sub.Creator.UserName
                });
            return Ok(subscriptions);
        }

        [HttpGet("[action]")]
        async public Task<ActionResult<List<Subscription>>> Subscribers() {
            int? userId = Helpers.GetCurrentUserId(HttpContext);

            if (!userId.HasValue)
                return BadRequest(new List<Subscription>());

            return Ok(await _context.Subscription.Where(sub => sub.CreatorId.Equals(userId.Value)).ToListAsync());
        }

        [HttpGet("{userName}/[action]")]
        async public Task<ActionResult<int>> SubscriberCount(string userName) {
            return await _context.Subscription.Include(sub => sub.Creator).Where(sub => sub.Creator.UserName == userName).CountAsync();
        }

        [HttpGet("Search/{query}")]
        async public Task<ActionResult<PublicUserDTO>> Search(string query) {
            var list = await _context.User.Where(u => u.UserName.Contains(query)).ToListAsync();
            return Ok(list.Select(u => u.ToPublicDTO()).ToList());
        }
    }
}
