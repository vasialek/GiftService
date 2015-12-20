using GiftService.Dal;
using GiftService.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{

    public interface ILogsBll
    {
        IEnumerable<LogBdo> GetLastLogs(int offset, int limit);
    }

    public class LogsBll : ILogsBll
    {
        private ILogsDal _logsDal = null;

        public LogsBll(ILogsDal logsDal)
        {
            if (logsDal == null)
            {
                throw new ArgumentNullException("logsDal");
            }

            _logsDal = logsDal;
        }

        public IEnumerable<LogBdo> GetLastLogs(int offset, int limit)
        {
            return _logsDal.GetLastLogs(offset, limit);
        }
    }
}
