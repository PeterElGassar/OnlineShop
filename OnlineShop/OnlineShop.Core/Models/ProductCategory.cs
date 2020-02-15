﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core
{
    public class ProductCategory
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public ProductCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
