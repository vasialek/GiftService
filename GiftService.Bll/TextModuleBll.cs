using GiftService.Dal;
using GiftService.Models.Texts;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{

    public interface ITextModuleBll
    {
        TextModule GetByLabel(string label, string culture);
    }

    public class TextModuleBll : ITextModuleBll
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

        private ITextModuleDal _textModuleDal = null;

        public TextModuleBll(ITextModuleDal textModuleDal)
        {
            if (textModuleDal == null)
            {
                throw new ArgumentNullException("textModuleDal");
            }

            _textModuleDal = textModuleDal;
        }

        public TextModule GetByLabel(string label, string culture)
        {
            string contentDir = BllFactory.Current.ConfigurationBll.Get().PathToPosContent;
            string file = System.IO.Path.Combine(contentDir, "html", String.Concat("_", label, "_", culture, ".html"));
            Logger.Info("Searching for TextModule (in files) by path: " + file);

            return new TextModule
            {
                Text = System.IO.File.ReadAllText(file)
            };

            //return _textModuleDal.GetByLabel(label, culture);
        }
    }
}
