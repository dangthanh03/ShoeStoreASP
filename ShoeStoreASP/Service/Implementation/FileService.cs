using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Service.Implementation
{
    public class FileService : IFileService
    {
        private readonly Cloudinary _cloudinary;

        private readonly IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment env, Cloudinary cloudinary)
        {
            this.environment = env;
            this._cloudinary = cloudinary;
        }
        [HttpPost]
        public Result<string> UploadImage(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                        };

                        var uploadResult = _cloudinary.Upload(uploadParams);

                        return Result<string>.Success(uploadResult.Url.ToString());
                    }
                }

                return Result<string>.Fail("No file provided for upload.");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết
                return Result<string>.Fail($"An error occurred: {ex.Message}");
            }
        }
        public Result<string> DeleteImageByUrl(string imageUrl)
        {
            try
            {
                var publicId = ExtractPublicIdFromUrl(imageUrl);

                if (publicId != null)
                {
                    var deletionParams = new DeletionParams(publicId)
                    {
                        ResourceType = ResourceType.Image
                    };

                    var deletionResult = _cloudinary.Destroy(deletionParams);

                    if (deletionResult.Result == "ok")
                    {
                        return Result<string>.Success("Image deleted successfully.");
                    }
                    else
                    {
                        return Result<string>.Fail($"Failed to delete image: {deletionResult.Error?.Message}");
                    }
                }
                else
                {
                    return Result<string>.Fail("Invalid image URL.");
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"An error occurred: {ex.Message}");
            }
        }

        private string ExtractPublicIdFromUrl(string imageUrl)
        {
            // Extract public ID from Cloudinary URL
            // Example: https://res.cloudinary.com/{cloud_name}/image/upload/{public_id}.{format}
            var parts = imageUrl.Split('/');
            var publicIdWithFormat = parts[parts.Length - 1];
            var publicId = publicIdWithFormat.Split('.')[0];
            return publicId;
        }
    }
}
