using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Exceptions
{
    public class BadResponseException : Exception
    {
        public BadResponseException()
            : this(String.Empty)
        {
        }

        public BadResponseException(string msg)
            : base(msg)
        {
        }
    }
}
