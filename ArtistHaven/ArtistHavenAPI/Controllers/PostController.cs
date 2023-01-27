using ArtistHaven.API.Data;
using ArtistHaven.API.Models;
using ArtistHaven.Shared.Models;
using ArtistHaven.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtistHaven.API.Controllers {
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PostController : ControllerBase {
        private readonly ArtistHavenAPIContext _context;

        public PostController(ArtistHavenAPIContext context) {
            _context = context;
        }

        [HttpPost]
        async public Task<ActionResult<BasicResponse>> Create(CreatePostModel model) {
            if (!ModelState.IsValid)
                return BadRequest(new BasicResponse());

            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new BasicResponse() { Message = "Id is null or not number" });

            Post post = new Post() {
                UserId = userId.Value,
                RequiredSubscriptionPower = model.RequiredSubscriptionPower,
                Text = model.Text,
            };
            Media? media = await _context.Media.Include(m => m.User).Where(m => m.User.Id == userId && m.Id == model.MediaId).FirstOrDefaultAsync();

            // User tried to include other user's media in their own post
            if (media == null)
                return BadRequest(new BasicResponse());

            post.Media = new List<PostMedia>() { new PostMedia() { Post = post, Media = media } };

            _context.Post.Add(post);
            foreach (var tagName in model.Tags) {
                Tag? tag = await _context.Tag.Where(t => t.Name.Equals(tagName)).FirstOrDefaultAsync();
                tag ??= new Tag() { Name = tagName };

                _context.PostTag.Add(new PostTag() {
                    Post = post,
                    Tag = tag
                });
            }

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                throw;
            }
            
            return Ok(new BasicResponse() { IsSuccessfull = true});
        }

        [HttpDelete("{id}")]
        async public Task<ActionResult<BasicResponse>> delete(int id) {
            if (!ModelState.IsValid)
                return BadRequest(new BasicResponse());

            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new BasicResponse() { Message = "Id is null or not number" });

            Post? post = await _context.Post.Where(post => post.Id == id && post.UserId == userId.Value).FirstOrDefaultAsync();
            if (post == null)
                return NotFound(new BasicResponse());

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return Ok(new BasicResponse() { IsSuccessfull = true});
        }
    }
}
