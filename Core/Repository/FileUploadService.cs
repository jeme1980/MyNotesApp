using MyNotesApp.Core.IRepository;
using MyNotesApp.Migrations;
using System.IO;

namespace MyNotesApp.Core.Repository
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<bool> DeleteFile(string oldfile)
        {
            try
            {
                string path = Path.GetFullPath(Path.Combine(_webHostEnvironment.WebRootPath, oldfile));
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> UploadFile(IFormFile file, string uploadPath)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(_webHostEnvironment.WebRootPath, uploadPath));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string newfilename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(path, newfilename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return newfilename;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        public async Task<string> UploadImage(IFormFile file,string dir)
        {
            try
            {
                var wwwPath = _webHostEnvironment.WebRootPath;
                var path = Path.Combine(wwwPath, dir);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return msg;
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                await file.CopyToAsync(stream);
                stream.Close();
                return newFileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private bool checkexetention(string uploadfilename)
        {
            bool allowed = true;
            string[] allowedexe = { ".png", ".jpg", "jpeg" };
            var exe = Path.GetExtension(uploadfilename).ToLowerInvariant();
            if (string.IsNullOrEmpty(exe) || !allowedexe.Contains(exe))
            {
                allowed = false;
            }
            return allowed;
        }
    }
}
