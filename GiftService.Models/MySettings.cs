using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class MySettings
    {
        public class MailAddresses
        {
            public string WebmasterEmail { get; set; }

            public string ManagerEmail { get; set; }
        }

        public class Options
        {
            public bool UseFakeDatabase { get; set; }
        }

        public MailAddresses Mails { get; set; }

        public Options WeOptions { get; set; }

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

        public int MinGiftCongratulationsLength { get; set; }

        public int MaxGiftCongratulationsLength { get; set; }

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

            Mails = new MailAddresses();
            Mails.ManagerEmail = "p.roglamer@gmail.com";
            Mails.WebmasterEmail = "p.roglamer@gmail.com";

            MinGiftCongratulationsLength = 5;
            MaxGiftCongratulationsLength = 230;

            WeOptions = new Options { UseFakeDatabase = false };
        }
    }
}
