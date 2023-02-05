using Microsoft.Extensions.Primitives;

namespace MyNotesApp.Core.IRepository
{
    public interface IFileUploadService
    {
        Task<string> UploadFile(IFormFile file,String uploadPath);
        Task<bool> DeleteFile(string oldfile);
        Task<string> UploadImage(IFormFile file,string dir);
    }
}
