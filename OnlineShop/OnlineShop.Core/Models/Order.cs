using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
           this.OrderItems = new List<OrderItem>();
        }
        public virtual ICollection<OrderItem> OrderItems { get; set; }



        public string OrderState { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //Address Details
        public string State { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }

    }
}
