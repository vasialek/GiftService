using GiftService.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.JsonModels
{
    /// <summary>
    /// Response from POS
    /// </summary>
    public class PaymentRequestValidationResponse : BaseResponse
    {
        /// <summary>
        /// Price user should pay (minor currency)
        /// </summary>
        public int RequestedAmountMinor { get; set; }

        public string CurrencyCode { get; set; }

        public string ProductName { get; set; }

        public string ProductDuration { get; set; }

        public UInt32 ProductValidTillTm { get; set; }

        public string ProductDescription { get; set; }

        public string PosName { get; set; }

        public string PosUrl { get; set; }

        public string PosAddress { get; set; }

        public string PosCity { get; set; }

        public string PhoneForReservation { get; set; }

        public string EmailForReservation { get; set; }

        public IList<ProductServiceLocation> Locations { get; set; }
    }
}
