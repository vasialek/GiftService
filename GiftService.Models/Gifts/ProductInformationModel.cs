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

        /// <summary>
        /// Information about product/service - name, price, valid, etc
        /// </summary>
        public ProductBdo Product { get; set; }

        public bool IsPaidOk { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
