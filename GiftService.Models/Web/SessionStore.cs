﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Web
{
    public class SessionStore
    {
        public int PosId { get; set; }

        /// <summary>
        /// Sent to payment system
        /// </summary>
        public string PaymentOrderNr { get; set; }
    }
}
