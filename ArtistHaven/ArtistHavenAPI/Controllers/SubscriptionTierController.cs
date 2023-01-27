using ArtistHaven.API.Data;
using ArtistHaven.API.Models;
using ArtistHaven.Shared;
using ArtistHaven.Shared.Models;
using ArtistHaven.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtistHaven.API.Controllers {
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SubscriptionTierController : ControllerBase {
        private readonly ArtistHavenAPIContext _context;

        public SubscriptionTierController(ArtistHavenAPIContext context) {
            _context = context;
        }

        [HttpPost]
        async public Task<ActionResult<BasicResponse>> Create(CreateSubscriptionTierModel model) {
            if (!ModelState.IsValid)
                return BadRequest();
            
            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new BasicResponse() { Message = "Id is null or not number" });

            _context.SubscriptionTier.Add(new SubscriptionTier() {
                SubscriptionPower = model.SubscriptionPower,
                UserId = userId.Value,
                Name = model.Name,
                Price = model.Price,
                Description = model.Description
            });

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                throw;
            }

            return Ok(new BasicResponse() { IsSuccessfull = true});
        }

        [HttpDelete("{subscriptionPower}")]
        async public Task<ActionResult<BasicResponse>> Delete(int subscriptionPower) {
            if (!ModelState.IsValid)
                return BadRequest(new BasicResponse());

            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new BasicResponse() { Message = "Id is null or not number" });

            SubscriptionTier? subTier = await _context.SubscriptionTier.FindAsync(subscriptionPower, userId.Value);
            if (subTier == null)
                return NotFound(new BasicResponse() { Message = "Subscription tier not found" });

            _context.SubscriptionTier.Remove(subTier);
            await _context.SaveChangesAsync();
            return Ok(new BasicResponse() { IsSuccessfull = true });
        }

        [HttpPut("{subscriptionPower}")]
        async public Task<ActionResult<BasicResponse>> Edit(int subscriptionPower, EditSubscriptionTierModel model) {
            if (!ModelState.IsValid)
                return BadRequest(new BasicResponse());

            int? userId = Helpers.GetCurrentUserId(HttpContext);
            if (!userId.HasValue)
                return BadRequest(new BasicResponse() { Message = "Id is null or not number" });

            SubscriptionTier? subTier = await _context.SubscriptionTier.FindAsync(subscriptionPower, userId.Value);
            if (subTier == null)
                return NotFound(new BasicResponse() { Message = "Subscription tier not found" });

            subTier.Description = model.Description;
            subTier.Name = model.Name;
            subTier.Price = model.Price;
            await _context.SaveChangesAsync();
            return Ok(new BasicResponse() { IsSuccessfull = true });
        }
    }
}
