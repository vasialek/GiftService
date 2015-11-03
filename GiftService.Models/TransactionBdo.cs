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

        public string ProductUid { get; set; }

        /// <summary>
        /// UID got from POS on payment request
        /// </summary>
        public string PosUserUid { get; set; }

        /// <summary>
        /// UID to communicate with payement systems
        /// </summary>
        public string PaySystemUid { get; set; }

        public decimal RequestedAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public int PaySystemId { get; set; }

        /// <summary>
        /// True when got response from payment system
        /// </summary>
        public bool IsPaymentProcessed { get; set; }

        public int ProductId { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of response from payment system or MinValue
        /// </summary>
        public DateTime PaySystemResponseAt { get; set; }
    }
}
