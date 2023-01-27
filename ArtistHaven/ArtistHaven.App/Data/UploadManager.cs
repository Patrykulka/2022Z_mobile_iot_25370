using ArtistHaven.App.Services;
using ArtistHaven.Shared;
using ArtistHaven.Shared.DTOs;
using ArtistHaven.Shared.Models;
using ArtistHaven.Shared.Responses;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ArtistHaven.App.Data {
    public class UploadManager {
        private IHttpClientService _httpClientService;

        public UploadManager(IHttpClientService httpClientService) {
            _httpClientService = httpClientService;
        }

        async public Task<UploadResponse> Upload(Stream stream) {
            MultipartFormDataContent form = new MultipartFormDataContent();
            try {
                stream.Position = 0;
                HttpContent content = new StreamContent(stream);
                var content2 = new ByteArrayContent(await content.ReadAsByteArrayAsync());
                content2.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(content2, "file", "file.png");
                return await _httpClientService.PostForm<UploadResponse>("/Upload", form);
            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return default;
            }
        }
    }
}
