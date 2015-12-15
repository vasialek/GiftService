using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public class DumpBll
    {
        public static string Dump(ProductBdo p)
        {
            // TODO: reflection
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0,-16}: `{1}`", "ID", p.Id).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PaySystemUid", p.PaySystemUid).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PosUserUid", p.PosUserUid).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "ProductUid", p.ProductUid).AppendLine();

            sb.AppendFormat("{0,-16}: `{1}`", "ProductName", p.ProductName).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "ProductPrice", p.ProductPrice).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "CurrencyCode", p.CurrencyCode).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "ProductDescription", p.ProductDescription).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "Remarks", p.Remarks).AppendLine();

            sb.AppendFormat("{0,-16}: `{1}`", "CustomerName", p.CustomerName).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "CustomerEmail", p.CustomerEmail).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "CustomerPhone", p.CustomerPhone).AppendLine();

            sb.AppendFormat("{0,-16}: `{1}`", "PosId", p.PosId).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PosName", p.PosName).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PosAddress", p.PosAddress).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PosCity", p.PosCity).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PosUrl", p.PosUrl).AppendLine();

            sb.AppendFormat("{0,-16}: `{1}`", "ValidFrom", p.ValidFrom).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "ValidTill", p.ValidTill).AppendLine();
            sb.AppendFormat("{0,-16}: `{1}`", "PaymentSystem", p.PaymentSystem).AppendLine();

            return sb.ToString();
        }
    }
}
