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
        PosBdo GetByPayerId(string payerId);
        IEnumerable<PosBdo> GetLastPos();
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
            p.Name = "Ritos Masažai";
            p.Description = "Rankos, suteikiančios pagalbą, švelnesnės negu besimeldžiančios lūpos";
            p.PayseraPayerId = "76457";
            p.PayseraPassword = "bd5d3446c45c365c8148f5580f717f67";
            p.PosUrl = new Uri("http://www.ritosmasazai.lt/");
            //p.ValidateUrl = new Uri("http://localhost:56620/Test/Validate");
            p.ValidateUrl = new Uri("http://www.ritosmasazai.lt/test.php/lt/price/info/posUid/");
            p.IsTest = false;
            p.UseTestPayment = false;
            _poses.Add(p);

            p = new PosBdo();
            p.Id = 6666;
            p.Name = "Test LocalShop";
            p.Description = "Just testing shop";
            p.PosUrl = new Uri("http://localhost:56620/");
            p.ValidateUrl = new Uri("http://localhost:56620/Test/Validate");
            //p.ValidateUrl = new Uri("http://www.ritosmasazai.lt/test.php/lt/price/info/posUid/");
            p.IsTest = true;
            p.UseTestPayment = true;
            _poses.Add(p);

            p = new PosBdo();
            p.Id = 1006;
            p.Name = "Knygynai.lt";
            p.Description = "Knygų ir knygynų paieška";
            p.PayseraPayerId = "80213";
            p.PayseraPassword = "87ff85458a1104dcacebe8eb9dec3a82";
            p.PosUrl = new Uri("http://knygynai.lt/");
            //p.ValidateUrl = new Uri("http://localhost:8079/gspclient/gsinfo.php?posUid=");
            p.ValidateUrl = new Uri("http://knygynai.lt/gsinfo.php?posUid=");
            p.IsTest = false;
            p.UseTestPayment = true;
            _poses.Add(p);
        }

        public PosBdo GetById(int posId)
        {
            Logger.InfoFormat("Searching for POS by ID #{0}", posId);
            return _poses.First(x => x.Id == posId);
        }

        public PosBdo GetByPayerId(string payerId)
        {
            Logger.InfoFormat("Searching for POS by payer ID #{0}", payerId);
            return _poses.First(x => payerId.Equals(x.PayseraPayerId));
        }

        public IEnumerable<PosBdo> GetLastPos()
        {
            Logger.Info("Getting list of last POS (not test)");
            return _poses.Where(x => x.IsTest == false).OrderByDescending(x => x.Id).ToList();
        }

    }
}
