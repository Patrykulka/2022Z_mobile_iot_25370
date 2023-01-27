using ArtistHaven.API.Data;
using ArtistHaven.API.Models;
using ArtistHaven.Shared;
using ArtistHaven.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtistHaven.API.Controllers {
    [Route("api/User/{userName}/[controller]")]
    [Authorize]
    [ApiController]
    public class SubscribeController : ControllerBase {
        private readonly ArtistHavenAPIContext _context;

        public SubscribeController(ArtistHavenAPIContext context) {
            _context = context;
        }

        [HttpPost("{subscriptionPower}")]
        async public Task<ActionResult<BasicResponse>> Subscribe(string userName, int subscriptionPower) {
            User? creator = await _context.User.Include(u => u.SubscriptionTiers).Where(u => u.UserName.Equals(userName)).FirstOrDefaultAsync();
            if (creator == null)
                return NotFound(new BasicResponse());

            int? id = Helpers.GetCurrentUserId(HttpContext);
            if (!id.HasValue)
                return BadRequest(new BasicResponse() { Message = "Id is null or not int" });

            if (id == creator.Id)
                return BadRequest(new BasicResponse() { Message = "You can't subscribe to yourself" });

            SubscriptionTier? st = creator.SubscriptionTiers.FirstOrDefault(st => st.SubscriptionPower == subscriptionPower);
            if (st == null)
                return NotFound(new BasicResponse() { Message = "Subscription Tier not found"});

            Subscription? sub = await _context.Subscription.FindAsync(id, creator.Id);
            if (sub == null)
                await _context.Subscription.AddAsync(new Subscription() {
                    CreatorId = creator.Id,
                    SubscriberId = id.Value,
                    SubscriptionTierPower = subscriptionPower
                });
            else {
                if (subscriptionPower < sub.SubscriptionTierPower)
                    return BadRequest(new BasicResponse() { Message = "You can't update your subscription to a subscription that has lower power."});
                else if (subscriptionPower == sub.SubscriptionTierPower)
                    return BadRequest(new BasicResponse() { Message = "You are already subscribed with that subscription." });
                sub.SubscriptionTierPower = subscriptionPower;
            }
            
            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                throw;
            }

            return Ok(new BasicResponse() { IsSuccessfull = true});
        }
    }
}
