using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class MySettings
    {
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
    }
}
