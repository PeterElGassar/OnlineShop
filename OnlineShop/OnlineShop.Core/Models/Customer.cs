using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models
{
    public class Customer : BaseEntity
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //Address Details
        public string State { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }



    }
}
