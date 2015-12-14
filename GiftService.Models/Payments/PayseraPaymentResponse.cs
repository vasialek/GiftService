using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Payments
{
    public class PayseraPaymentResponse
    {
        public string OrderId { get; set; }

        public string ProjectId { get; set; }

        public decimal AmountToPay { get; set; }
        public string CurrencyCode { get; set; }
        public string Payment { get; set; }

        public decimal PayAmount { get; set; }
        public string PayText { get; set; }
        public string PayCurrencyCode { get; set; }

        public string Country { get; set; }
        public bool IsTestPayment { get; set; }

        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPhone { get; set; }
        public string Remarks { get; set; }

        public string Ip { get; set; }
        public string BrowserAgent { get; set; }
        public string File { get; set; }
        public string Version { get; set; }
        public string Lang { get; set; }
        public string m_pay_restored { get; set; }
        public string Status { get; set; }
        public string RequestId { get; set; }

        public PayseraPaymentResponse()
        {
            IsTestPayment = true;
        }
    }
}
