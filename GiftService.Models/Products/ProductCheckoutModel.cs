using GiftService.Models;
using GiftService.Models.Infrastructure;
using GiftService.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GiftService.Models.Products
{
    public class ProductCheckoutModel
    {
        public string PosUserUid { get; set; }

        public string PaySystemUid { get; set; }

        public PaymentSystems PaymentSystem { get; set; }

        public decimal RequestedAmount { get; set; }

        public string CurrencyCode { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductDuration { get; set; }

        public DateTime ProductValidTill { get; set; }

        [Display(Name = "Vardas")]
        public string CustomerName { get; set; }

        [Display(Name = "Tel. nr.")]
        [PhoneByCountry("lt", ErrorMessage = "Neteisingas tel. nr.")]
        public string CustomerPhone { get; set; }

        [Display(Name = "El. paštas")]
        [Required(ErrorMessage = "Privalomas laukas")]
        [EmailAddress(ErrorMessage = "Neteisingas el. pašto adresas")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Pastabos")]
        public string Remarks { get; set; }

        public int LocationId { get; set; }

        public IEnumerable<ProductServiceLocation> Locations { get; set; }

    }
}
