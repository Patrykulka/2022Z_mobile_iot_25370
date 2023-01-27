using ArtistHaven.App.Services;
using ArtistHaven.Shared;
using ArtistHaven.Shared.DTOs;
using ArtistHaven.Shared.Models;
using ArtistHaven.Shared.Responses;
using System.Collections.ObjectModel;

namespace ArtistHaven.App.Data {
    public class UserManager {
        private IHttpClientService _httpClientService;

        public UserManager(IHttpClientService httpClientService) {
            _httpClientService = httpClientService;
        }

        async public Task<PrivateUserDTO> Get() {
            return await _httpClientService.Get<PrivateUserDTO>($"/User");
        }

        async public Task<PublicUserDTO> Get(string userName) {
            return await _httpClientService.Get<PublicUserDTO>($"/User/{userName}");
        }

        async public Task<List<SubscriptionTierDTO>> GetSubscriptionTiers(string userName) {
            return await _httpClientService.Get<List<SubscriptionTierDTO>>($"/User/{userName}/SubscriptionTiers");
        }

        async public Task<BasicResponse> CreateSubTier(CreateSubscriptionTierModel model) {
            return await _httpClientService.Post<BasicResponse>("/SubscriptionTier", model);
        }

        async public Task<BasicResponse> EditSubTier(int subPower, EditSubscriptionTierModel model) {
            return await _httpClientService.Put<BasicResponse>($"/SubscriptionTier/{subPower}", model);
        }

        async public Task<BasicResponse> DeleteSubTier(int subPower) {
            return await _httpClientService.Delete<BasicResponse>($"/SubscriptionTier/{subPower}");
        }

        async public Task<BasicResponse> CreatePost(CreatePostModel model) {
            return await _httpClientService.Post<BasicResponse>($"/Post", model);
        }

        async public Task<List<PostDTO>> GetPost(string userName) {
            return await _httpClientService.Get<List<PostDTO>>($"/User/{userName}/Posts");
        }

        async public Task<List<SubscriptionDTO>> GetSubscriptions() {
            return await _httpClientService.Get<List<SubscriptionDTO>>("/User/Subscriptions");
        }

        async public Task<List<PublicUserDTO>> FindUsers(string query) {
            return await _httpClientService.Get<List<PublicUserDTO>>($"/User/Search/{query}");
        }

        async public Task<BasicResponse> Subscribe(string userName, int subscriptionPower) {
            return await _httpClientService.Post<BasicResponse>($"/User/{userName}/Subscribe/{subscriptionPower}");
        }
    }
}
