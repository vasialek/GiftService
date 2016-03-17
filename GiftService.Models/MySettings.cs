using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class MySettings
    {
        public string ProjectDomain { get; set; }

        public string ProjectName { get; set; }

        public bool UseTestPayment { get; set; }

        public Uri PayseraPaymentUrl { get; set; }

        public int MaxLengthOfPayseraNote { get; set; }

        public int LengthOfPosUid { get; set; }

        public int LengthOfOrderId { get; set; }

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
            // E25-48662E
            LengthOfOrderId = 10;

            ProjectDomain = "www.DovanuKuponai.com";
            ProjectName = "DovanuKuponai.com";
            UseTestPayment = false;
            PayseraPaymentUrl = new Uri("https://www.mokejimai.lt/pay/");
            MaxLengthOfPayseraNote = 255;
        }
    }
}
