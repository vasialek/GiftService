using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Exceptions
{
    public class TransactionDoesNotExist : Exception
    {
        public string PaySystemUid { get; set; }

        public TransactionDoesNotExist()
            : this("", "")
        {
        }

        public TransactionDoesNotExist(string msg, string paySystemUid)
            : base(msg)
        {
            PaySystemUid = paySystemUid;
        }
    }
}
