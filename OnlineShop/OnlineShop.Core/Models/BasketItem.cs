using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models
{
    public class BasketItem : BaseEntity
    {

        public string BasketId { get; set; }

        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }


    }
}
