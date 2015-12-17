using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftService.Models
{
    public class ProductInformationModel
    {
        public string GiftUid { get; set; }

        /// <summary>
        /// Information about POS provided this product/service
        /// </summary>
        public PosBdo Pos { get; set; }

        public string PosAddress
        {
            get
            {
                if (Product != null)
                {
                    return String.Join(", ", new string[] { Product.PosAddress, Product.PosCity });
                }
                return "";
            }
        }

        /// <summary>
        /// Information about product/service - name, price, valid, etc
        /// </summary>
        public ProductBdo Product { get; set; }

        public PaymentStatusIds PaymentStatus { get; set; }

        public bool IsPaidOk { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
