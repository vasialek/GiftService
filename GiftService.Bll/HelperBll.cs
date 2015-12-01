using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{

    public interface IHelperBll
    {
        DateTime UnixStart { get; }
        DateTime ConvertFromUnixTimestamp(UInt32 timestamp);
    }

    public class HelperBll : IHelperBll
    {
        public DateTime UnixStart
        {
            get
            {
                return new DateTime(1970, 1, 1);
            }
        }

        public DateTime ConvertFromUnixTimestamp(uint timestamp)
        {
            return UnixStart.AddSeconds(timestamp);
        }
    }
}
