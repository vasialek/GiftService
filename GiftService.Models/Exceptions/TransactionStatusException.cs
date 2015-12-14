using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Exceptions
{
    public class TransactionStatusException : Exception
    {
        public TransactionStatusException()
            : this("")
        {
        }

        public TransactionStatusException(string msg)
            : base(msg)
        {
        }
    }
}
