using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Pos
{
    public class PosPdfLayout
    {
        public int PosId { get; set; }

        /// <summary>
        /// Header image name or NULL
        /// </summary>
        public string HeaderImage { get; set; }

        /// <summary>
        /// Footer image name or NULL
        /// </summary>
        public string FooterImage { get; set; }
    }
}
