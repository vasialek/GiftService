using GiftService.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public class BllFactory
    {
        private static BllFactory _bllFactory = null;

        private IConfigurationBll _configurationBll = null;
        private IProductsBll _productsBll = null;
        private IPdfBll _pdfBll = null;

        private BllFactory()
        {
        }

        public static BllFactory Current
        {
            get
            {
                if (_bllFactory == null)
                {
                    _bllFactory = new BllFactory();
                }
                return _bllFactory;
            }
        }


        public IConfigurationBll ConfigurationBll
        {
            get
            {
                if (_configurationBll == null)
                {
                    _configurationBll = new ConfigurationBll();
                }
                return _configurationBll;
            }
        }

        public IProductsBll GiftsBll
        {
            get
            {
                if (_productsBll == null)
                {
                    _productsBll = new ProductsBll(DalFactory.Current.ProductsDal, DalFactory.Current.PosDal);
                }
                return _productsBll;
            }
        }


        public IPdfBll PdfBll
        {
            get
            {
                if (_pdfBll == null)
                {
                    _pdfBll = new PdfBll(ConfigurationBll, DalFactory.Current.ProductsDal);
                }
                return _pdfBll;
            }
        }

    }
}
