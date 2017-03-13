using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public enum GsErrorCodes
    {
        None = 0,
        CongratulationsTextIsToShort = 200,
        CongratulationsTextIsTooLong,
        CongratulationsTextIsEmpty
    }
}
