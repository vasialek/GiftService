using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Exceptions
{
    public class IncorrectPaymentParamersException : Exception
    {

        public IncorrectPaymentParamersException()
            : this("")
        {
        }

        public IncorrectPaymentParamersException(string msg)
            : base(msg)
        {
        }
    }
}
