using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.ViewModels
{
   public class BasketItemVM
    {

        public string BasketItemId { get; set; }
        public int Quantity { get; set; }

        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

    }
}
