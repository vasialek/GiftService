using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class MySettings
    {
        public bool UseTestPayment { get; set; }

        public string PayseraPassword { get; set; }

        public int PayseraProjectId { get; set; }

        public Uri PayseraPaymentUrl { get; set; }

        public int LengthOfPosUid { get; set; }

        /// <summary>
        /// How many symbols to take from UID to use as directory name
        /// </summary>
        public int LengthOfPdfDirectoryName { get; set; }

        public string PathToPdfStorage { get; set; }

        /// <summary>
        /// Path to all content related to POS (CSS, images, PDF)
        /// </summary>
        public string PathToPosContent { get; set; }

        public MySettings()
        {
            UseTestPayment = true;
            PayseraPassword = "8ad52eb96c5f3337ba70f9b251310984";
            PayseraProjectId = 76457;
            PayseraPaymentUrl = new Uri("https://www.mokejimai.lt/pay/");
        }
    }
}
