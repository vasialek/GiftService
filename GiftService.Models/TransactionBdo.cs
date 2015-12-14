using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class TransactionBdo
    {
        public int Id { get; set; }

        public int PosId { get; set; }

        public bool IsTestPayment { get; set; }

        public PaymentStatusIds PaymentStatus { get; set; }

        public string ProductUid { get; set; }

        /// <summary>
        /// UID got from POS on payment request
        /// </summary>
        public string PosUserUid { get; set; }

        /// <summary>
        /// UID to communicate with payement systems
        /// </summary>
        public string PaySystemUid { get; set; }

        public PaymentSystems PaymentSystem { get; set; }

        public int ProjectId { get; set; }

        public decimal RequestedAmount { get; set; }

        public string RequestedCurrencyCode { get; set; }

        public decimal PaidAmount { get; set; }

        public string PaidCurrencyCode { get; set; }

        /// <summary>
        /// True when got response from payment system
        /// </summary>
        public bool IsPaymentProcessed { get; set; }

        public int ProductId { get; set; }

        public string PayerName { get; set; }

        public string PayerLastName { get; set; }

        public string PayerPhone { get; set; }

        public string PayerEmail { get; set; }

        public string Remarks { get; set; }

        /// <summary>
        /// Gets/sets response from payment system
        /// </summary>
        public string ResponseFromPaymentSystem { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Which bank was used to pay
        /// </summary>
        public string PaidThrough { get; set; }

        /// <summary>
        /// Date of response from payment system or MinValue
        /// </summary>
        public DateTime PaySystemResponseAt { get; set; }
    }
}
