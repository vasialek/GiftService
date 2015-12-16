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
            UseTestPayment = false;
            PayseraPassword = "bd5d3446c45c365c8148f5580f717f67";
            PayseraProjectId = 76457;
            PayseraPaymentUrl = new Uri("https://www.mokejimai.lt/pay/");
        }
    }
}
