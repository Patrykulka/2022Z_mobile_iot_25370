using ArtistHaven.API.Data;
using ArtistHaven.API.Services;
using ArtistHaven.Shared.Models;
using ArtistHaven.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace ArtistHaven.API.Controllers {
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UploadController : ControllerBase {
        private readonly ArtistHavenAPIContext _context;
        private readonly IImageService _uploadService;

        public UploadController(ArtistHavenAPIContext context, IImageService uploadService) {
            _context = context;
            _uploadService = uploadService;
        }

        [HttpPost]
        async public Task<ActionResult<UploadResponse>> Upload(IFormFile file) {
            if (!ModelState.IsValid)
                return BadRequest(new UploadResponse());

            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new UploadResponse() { Message = "Id is null or not number" });

            if (await _context.Media.CountAsync() > 1000)
                return BadRequest(new UploadResponse() { Message = "Too many files in db" });

            // 10 MB limit
            if (file.Length > 1024 * 1024 * 10)
                return BadRequest(new UploadResponse() { Message = "File is too large" });
                
            string fileId = Guid.NewGuid().ToString();
            string filePath = $"{userId.Value}/{fileId}.jpg";
            await _uploadService.Upload(filePath, file);
            Models.Media media = new Models.Media() {
                UserId = userId.Value,
                FileName = HttpUtility.UrlEncode(file.FileName),
                Path = filePath
            };
            _context.Media.Add(media);
            await _context.SaveChangesAsync();
            return Ok(new UploadResponse() { IsSuccessfull = true, MediaID = media.Id });
        }
    }
}
