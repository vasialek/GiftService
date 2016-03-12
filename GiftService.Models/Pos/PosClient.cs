using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Pos
{
    public class PosClient
    {
        public int PosId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
    }
}
