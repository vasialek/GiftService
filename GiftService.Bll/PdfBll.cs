using GiftService.Dal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IPdfBll
    {
        byte[] GetProductPdf(string productUid);
    }

    public class PdfBll : IPdfBll
    {
        private IConfigurationBll _configurationBll = null;
        private IProductsDal _productsDal = null;

        public PdfBll(IConfigurationBll configurationBll, IProductsDal productsDal)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }
            if (productsDal == null)
            {
                throw new ArgumentNullException("productsDal");
            }

            _configurationBll = configurationBll;
            _productsDal = productsDal;
        }

        public byte[] GetProductPdf(string productUid)
        {
            var p = _productsDal.GetProductByUid(productUid);

            string pathToPdf = Path.Combine(_configurationBll.Get().PathToPdfStorage, "test.pdf");

            return File.ReadAllBytes(pathToPdf);
        }
    }
}
