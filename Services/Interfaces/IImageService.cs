using TheBugTracker.Models.Enums;

namespace TheBugTracker.Services.Interfaces
{
        public interface IImageService
        {
            public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file);

            public string? ConvertByteArrayToFile(byte[] fileData, string? extension, DefaultImage defaultImage);
        }
    
}
