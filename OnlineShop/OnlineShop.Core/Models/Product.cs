using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Core.Models
{
    public class Product : BaseEntity
    {
        // this Class Already Inhertince Id Prop From Base Class
        //public string Id { get; set; }


        [StringLength(30, ErrorMessage = "The maximum length of Name is 30 Charachter")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        public string Description { get; set; }


        [Range(0, 10000)]
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }
        //=======================Edit=====================
        //public string Slug { get; set; }

        [ForeignKey("ProductCategory")]
        public string ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }



    }
}
