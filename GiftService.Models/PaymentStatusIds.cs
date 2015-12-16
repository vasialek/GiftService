using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public enum PaymentStatusIds
    {
        NotProcessed = 0,
        WaitingForPayment = 1,
        UserCancelled = 2,
        PaidOk = 3,
        AcceptedButNotExecuted = 4
    }
}
