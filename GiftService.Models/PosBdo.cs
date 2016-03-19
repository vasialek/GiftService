using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class PosBdo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Uri PosUrl { get; set; }

        public string PayseraPayerId { get; set; }

        public string PayseraPassword { get; set; }

        /// <summary>
        /// POS should confirm that this request is valid and provide data for this order
        /// </summary>
        public Uri ValidateUrl { get; set; }

        /// <summary>
        /// Callback after successful payment to POS
        /// </summary>
        public Uri AcceptUrl { get; set; }

        /// <summary>
        /// When user cancels order
        /// </summary>
        public Uri CancelUrl { get; set; }

        /// <summary>
        /// General errors from payment system
        /// </summary>
        public Uri CallbackUrl { get; set; }

        public bool IsTest { get; set; }

        /// <summary>
        /// Gets/sets whether payment is real or test
        /// </summary>
        public bool UseTestPayment { get; set; }
    }
}
