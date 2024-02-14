using ShoeStoreASP.Models.Domain;

namespace ShoeStoreASP.Service.Abstract
{
    public interface IFileService
    {
        public Result<string> UploadImage(IFormFile file);
        public Result<string> DeleteImageByUrl(string imageUrl);
    }
}
