using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace ArtistHaven.API.Services {
    public interface IImageService {
        public Task Upload(string fileName, IFormFile file);
        public Task<byte[]> Download(string fileName);
    }
    public class ImageService : IImageService {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _imageContainerClient;

        public ImageService(BlobServiceClient blobServiceClient) {
            _blobServiceClient = blobServiceClient;
            _imageContainerClient = blobServiceClient.GetBlobContainerClient("images");
        }

        public async Task Upload(string fileName, IFormFile file) {
            using (var stream = new MemoryStream()) {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await _imageContainerClient.UploadBlobAsync(fileName, stream);
            }
        }

        public async Task<byte[]> Download(string fileName) {
            using (var stream = new MemoryStream()) {
                await _imageContainerClient.GetBlobClient(fileName).DownloadToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}
