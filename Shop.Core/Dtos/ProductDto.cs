using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Shop.Core.Dtos
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string Transmission { get; set; }
        public int Power { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public List<IFormFile> Files { get; set; }
        public IEnumerable<ExistingFilePathDto> ExistingFilePaths { get; set; } 
            = new List<ExistingFilePathDto>();
    }
}
