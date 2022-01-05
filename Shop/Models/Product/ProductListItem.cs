using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Product
{
    public class ProductListItem
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
    }
}
