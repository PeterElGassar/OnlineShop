using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.ViewModels
{
    public class ProductManagerVM
    {
        [Required]
        public Product Product { get; set; }

        public IEnumerable<ProductCategory> Categories { get; set; }
    }
}
