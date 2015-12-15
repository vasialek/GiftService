using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Payments
{
    public class PaidThroughSystem
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public PaidThroughSystem(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
