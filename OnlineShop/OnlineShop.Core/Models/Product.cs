using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineShop.Core.Models
{
    public class Product
    {
        public string Id { get; set; }


        [StringLength(30, ErrorMessage = "The maximum length of Name is 30 Charachter")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }


        /// <summary>
        /// In this constructor i will Genrat Id  
        /// </summary>
        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
