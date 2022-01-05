using Shop.Core.Domain;
using Shop.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface IFileServices : IApplicationService
    {
        Task<ExistingFilePath> ProcessUploadFile(ProductDto dto, Product product);

        Task<ExistingFilePath> RemoveImage(ExistingFilePathDto dto);
    }
}
