using GiftService.Models;
using GiftService.Models.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IConfigurationBll
    {
        MySettings Get();
        PosPdfLayout GetPdfLayout(int posId);
        string GetDirectoryNameByUid(string productUid);
    }
    public class ConfigurationBll : IConfigurationBll

    {
        private MySettings _settings = null;

        public MySettings Get()
        {
            if (_settings == null)
            {
                _settings = new MySettings();
                //_settings.PathToPdfStorage = "c:\\temp\\giftservice\\";
                _settings.PathToPdfStorage = "E:\\web\\dovanuku\\Content\\coupons\\";
                _settings.PathToPosContent = "E:\\web\\dovanuku\\Content\\";
                //_settings.PathToPosContent = "c:\\_projects\\GiftService\\GiftService.Web\\Content\\";
                _settings.LengthOfPosUid = 32;
                _settings.LengthOfPdfDirectoryName = 5;
            }
            return _settings;
        }

        public string GetDirectoryNameByUid(string productUid)
        {
            if (String.IsNullOrEmpty(productUid))
            {
                throw new ArgumentNullException("Product UID must be non-empty");
            }

            int uidLength = Get().LengthOfPosUid;
            if (uidLength != productUid.Length)
            {
                throw new ArgumentOutOfRangeException("productUid", "Product UID must be exactly " + uidLength + " symbols");
            }

            return productUid.Substring(0, Get().LengthOfPdfDirectoryName);
        }

        public PosPdfLayout GetPdfLayout(int posId)
        {
            return new PosPdfLayout
            {
                PosId = 1005,
                HeaderImage = "header.jpg",
                FooterImage = "footer.jpg"
            };
        }
    }
}
