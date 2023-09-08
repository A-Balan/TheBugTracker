using TheBugTracker.Models.Enums;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class ImageService : IImageService
    {
        private readonly string? _defaultProjectImage = "/img/sample.png";
        private readonly string? _defaultUserImage = "/img/defaultuser.png";
      

        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage)
        {

            try
            {
                if (fileData == null || fileData.Length == 0)
                {

                    switch (defaultImage)
                    {
                        case DefaultImage.BTUserImage: return _defaultUserImage;

                        case DefaultImage.ProjectImage: return _defaultProjectImage;
                     
                    }

                }

                string? imageBase64Data = Convert.ToBase64String(fileData!);
                imageBase64Data = string.Format($"data:{extension};base64,{imageBase64Data}");

                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();
                await file!.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();

                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
