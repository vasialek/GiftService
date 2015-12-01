using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Products
{
    public class ProductCheckoutModel
    {
        public string PosUserUid { get; set; }

        public string PaySystemUid { get; set; }

        public decimal RequestedAmount { get; set; }

        public string CurrencyCode { get; set; }

        public string ProductName { get; set; }

        public string ProductDuration { get; set; }

        public DateTime ProductValidTill { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer phone")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Customer e-mail")]
        public string CustomerEmail { get; set; }

        public string Remarks { get; set; }

        public IEnumerable<ProductServiceLocation> Locations { get; set; }

    }
}
