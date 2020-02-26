using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.ViewModels
{
    public class BasketSummaryVM
    {
        public int BasketCount { get; set; }
        public decimal BasketTotal { get; set; }

        public BasketSummaryVM()
        {

        }

        public BasketSummaryVM(int BasketCount, decimal BasketTotal)
        {
            this.BasketCount = BasketCount;
            this.BasketTotal = BasketTotal;
        }
    }
}
