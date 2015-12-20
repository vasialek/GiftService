using GiftService.Models.Logs;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{

    public interface ILogsDal
    {
        IEnumerable<LogBdo> GetLastLogs(int offset, int limit);
    }

    public class LogsDal : ILogsDal
    {
        private ILog _logger = null;
        private ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(GetType());
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }

        public IEnumerable<LogBdo> GetLastLogs(int offset, int limit)
        {
            var list = new List<LogBdo>();
            IEnumerable<gslog> logs = null;
            try
            {
                using (var db = new GiftServiceEntities())
                {
                    logs = db.gslogs.OrderByDescending(x => x.id)
                        .Skip(offset)
                        .Take(limit)
                        .ToList();
                }

                foreach (var l in logs)
                {
                    list.Add(new LogBdo
                    {
                        Id = l.id,
                        Message = l.message,
                        Thread = l.thread,
                        CreatedAtServer = l.date,
                        Exception = l.exception,
                        Level = l.level
                    });
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return list;
        }
    }
}
