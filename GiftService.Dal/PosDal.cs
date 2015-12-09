using GiftService.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{
    public interface IPosDal
    {
        PosBdo GetById(int posId);
    }

    public class PosDal : IPosDal
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

        private List<PosBdo> _poses = new List<PosBdo>();

        public PosDal()
        {
            PosBdo p = new PosBdo();
            p.Id = 1005;
            p.Name = "Ritos Masazai";
            p.PosUrl = new Uri("http://www.ritosmasazai.lt/");
            //p.ValidateUrl = new Uri("http://localhost:56620/Test/Validate");
            p.ValidateUrl = new Uri("http://www.ritosmasazai.lt/test.php/lt/price/info/posUid/");
            _poses.Add(p);
        }

        public PosBdo GetById(int posId)
        {
            Logger.InfoFormat("Searching for POS by ID #{0}", posId);
            return _poses.First(x => x.Id == posId);
        }
    }
}
