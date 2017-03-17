using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftService.Models.Auth;

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

        public static string Dump(IEnumerable<RoleModel> roles, string title)
        {
            int maxLenOfName = roles.Max(x => x.Name.Length);
            // +---------------------------+
            // | Role name | Is set | Guid |
            int len = maxLenOfName + 53;
            string separator = String.Concat("+".PadRight(len - 2, '-'), "+");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(separator);

            if (String.IsNullOrEmpty(title) == false)
            {
                sb.AppendFormat("|{0}|", title.PadRight(len - 3));
                sb.AppendLine();
                sb.AppendLine(separator);
            }

            sb.AppendFormat("| {0} | Is set | {1} |", "    Role name".PadRight(maxLenOfName), "          Role ID".PadRight(36));
            sb.AppendLine();
            sb.AppendLine(separator);

            foreach (var r in roles)
            {
                sb.AppendFormat("| {0} |   {1}    | {2} |", r.Name.PadRight(maxLenOfName), r.Selected ? "+" : "-", r.Id);
                sb.AppendLine();
            }

            sb.AppendLine(separator);

            return sb.ToString();
        }
    }
}
