using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using Shop.Core.Dtos;
using Shop.Core.ServiceInterface;
using Shop.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FileServices
        (
        ShopDbContext context,
        IWebHostEnvironment env
        )
            {
                _context = context;
                _env = env;
            }


        public async Task<ExistingFilePath> ProcessUploadFile(ProductDto dto, Product product)
        {
            string uniqueFileName = null;

            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_env.WebRootPath + "\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_env.WebRootPath + "\\multipleFileUpload\\");
                }

                foreach (var photo in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "multipleFileUpload");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);

                        ExistingFilePath path = new ExistingFilePath
                        {
                            Id = Guid.NewGuid(),
                            FilePath = uniqueFileName,
                            ProductId = product.Id
                        };

                        await _context.ExistingFilePath.AddAsync(path);
                        return path;
                    }
                }
            }

            return null;
        }


        public async Task<ExistingFilePath> RemoveImage(ExistingFilePathDto dto)
        {
            var photoId = await _context.ExistingFilePath
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            _context.ExistingFilePath.Remove(photoId);
            await _context.SaveChangesAsync();

            return photoId;
        }
    }
}
