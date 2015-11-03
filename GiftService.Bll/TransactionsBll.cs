using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface ITransactionsBll
    {
        TransactionBdo StartTransaction(string posUserUid, int posId);
    }

    public class TransactionsBll : ITransactionsBll
    {
        public TransactionBdo StartTransaction(string posUserUid, int posId)
        {
            var t = new TransactionBdo();

            t.PosUserUid = posUserUid;
            t.PosId = posId;
            t.CreatedAt = DateTime.UtcNow;

            t.PaySystemUid = GenerateUid();

            return t;
        }

        private string GenerateUid()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
