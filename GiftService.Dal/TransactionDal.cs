using AutoMapper;
using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{

    public interface ITransactionDal
    {
        void StartTransaction(TransactionBdo t);
        TransactionBdo GetTransactionByPaySystemUid(string paySystemUid);
    }

    public class TransactionDal : ITransactionDal
    {
        public TransactionBdo GetTransactionByPaySystemUid(string paySystemUid)
        {
            TransactionBdo transaction = null;
            transaction t = null;
            using (var db = new GiftServiceEntities())
            {
                t = db.transactions.First(x => x.pay_system_uid.Equals(paySystemUid));
            }

            if (t != null)
            {
                AutoMapperConfigDal.SetMappingTypeFromLowerToPascal();
                Mapper.CreateMap<transaction, TransactionBdo>();
                transaction = Mapper.Map<TransactionBdo>(t);
            }

            return transaction;
        }

        public void StartTransaction(TransactionBdo t)
        {
            Mapper.CreateMap<TransactionBdo, transaction>();
            var transaction = Mapper.Map<transaction>(t);
            using (var db = new GiftServiceEntities())
            {
                db.transactions.Add(transaction);
                db.SaveChanges();
            }
        }
    }
}
