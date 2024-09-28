using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Common.Services.Attachments
{
    internal class AttachmentService : IAttachmentService
    {
        private List<string> _allowedExtentions = new List<string>() { ".png", ".jpg", ".jpeg" };
        private const int _allowedMaxSize = 2_097_152;
        public string? Upload(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName); // formFile.FileName ==> File+extension
                                                              // formFile.Name ==> extension

            if (!_allowedExtentions.Contains(extension) || file.Length > _allowedMaxSize)
                return null;

            //var folderPath = $"{Directory.GetCurrentDirectory}\\wwwroot\\files\\{folderName}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            var fileName = $"{Guid.NewGuid()}{extension}"; // Must be Unique

            var filePath = Path.Combine(folderPath, fileName); // File Location Placed

            // Stream: Data Per Time

            using var fileStream = new FileStream(filePath, FileMode.Create);

            //using var fileStream = File.Create(filePath);

            file.CopyTo(fileStream);

            return fileName;

        }
        public bool Delete(string filePath)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}
