using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Payments
{
    public class PayseraPaymentRequest
    {
        public const string Version = "1.6";

        public enum Languages { LIT, LAV, EST, RUS, ENG, GER, POL }

        public enum Countries { LT, EE, LV, GB, PL, DE }

        public string PayseraProjectPassword { get; set; }

        public string ProjectId { get; set; }

        public string OrderId { get; set; }

        public string AcceptUrl { get; set; }

        public string CancelUrl { get; set; }

        public string CallbackUrl { get; set; }

        public Languages Language { get; set; }

        public decimal AmountToPay { get; set; }

        public string CurrencyCode { get; set; }

        public Countries Country { get; set; }

        public string PayText { get; set; }

        public string CustomerName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerZip { get; set; }

        public string CustomerCountryCode { get; set; }

        public string Remarks { get; set; }

        public string Ip { get; set; }

        public string BrowserAgent { get; set; }

        /// <summary>
        /// Unknown file
        /// </summary>
        public string File { get; set; }

        public bool IsTestPayment { get; set; }

        public string RequestedPaymentMehtods { get; set; }

        public PayseraPaymentRequest()
        {
            // By default it is test payment
            IsTestPayment = true;
        }
    }
}
