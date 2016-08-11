using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Exceptions
{
    public class InvalidProductException : Exception
    {
        public enum Reasons
        {
            ProductExpired = 1,
            ProductIsGiftAlready
        }

        public Reasons Reason { get; set; }

        public InvalidProductException(string msg, Reasons reason)
            : base(msg)
        {
            Reason = reason;
        }
    }
}
