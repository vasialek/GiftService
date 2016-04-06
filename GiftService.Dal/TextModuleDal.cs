using GiftService.Models.Texts;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{
    public interface ITextModuleDal
    {
        TextModule GetByLabel(string label, string culture);
    }

    public class TextModuleDal : ITextModuleDal
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

        public TextModule GetByLabel(string label, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
