using GiftService.Models;
using GiftService.Models.Payments;
using GiftService.Models.Pos;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        IEnumerable<PaidThroughSystem> GetPaidThroughSystems();
        PaidThroughSystem GetPaidThroughSystemByCode(string code);
        string GetPaidThroughSystemName(string code);
    }
    public class ConfigurationBll : IConfigurationBll

    {
        private MySettings _settings = null;

        public MySettings Get()
        {
            if (_settings == null)
            {
                _settings = new MySettings();

                string s;
                bool b;

                s = ConfigurationManager.AppSettings["UseTestPayment"];
                if (Boolean.TryParse(s, out b) == false)
                {
                    b = true;
                }
                _settings.UseTestPayment = b;

                string webContentDir = GetMandatoryFromSettings("WebContentDir");
                _settings.PathToPosContent = webContentDir;
                _settings.PathToPdfStorage = System.IO.Path.Combine(webContentDir, "coupons");

                _settings.PayseraPassword = GetMandatoryFromSettings("PayseraPassword");
                _settings.PayseraPaymentUrl = new Uri(GetMandatoryFromSettings("PayseraPaymentGate"));
                _settings.PayseraProjectId = int.Parse(GetMandatoryFromSettings("PayseraProjectId"));

                //_settings.PathToPdfStorage = "c:\\temp\\giftservice\\";
                //_settings.PathToPdfStorage = "E:\\web\\dovanuku\\Content\\coupons\\";
                //_settings.PathToPosContent = "E:\\web\\dovanuku\\Content\\";
                //_settings.PathToPosContent = "c:\\_projects\\GiftService\\GiftService.Web\\Content\\";
                _settings.LengthOfPosUid = 32;
                _settings.LengthOfPdfDirectoryName = 5;
            }
            return _settings;
        }

        protected string GetMandatoryFromSettings(string key)
        {
            string s = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrEmpty(s))
            {
                throw new ConfigurationErrorsException("Missing key `" + key + "` in Web.config");
            }
            return s;
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

        List<PaidThroughSystem> _systems = null;
        public IEnumerable<PaidThroughSystem> GetPaidThroughSystems()
        {
            if (_systems == null)
            {
                _systems = new List<PaidThroughSystem>();
                _systems.Add(new PaidThroughSystem("mb", "JSC bank \"Medicinos Bankas\""));
                _systems.Add(new PaidThroughSystem("lku", "Lithuanian credit union"));
                _systems.Add(new PaidThroughSystem("sb", "JSC bank \"Šiaulių bankas\""));
                _systems.Add(new PaidThroughSystem("hanza", "JSC bank \"Swedbank\""));
                _systems.Add(new PaidThroughSystem("nord", "JSC bank \"DNB\""));
                _systems.Add(new PaidThroughSystem("sampo", "JSC bank \"Danske\""));
                _systems.Add(new PaidThroughSystem("nordealt", "JSC bank \"Nordea\""));
                _systems.Add(new PaidThroughSystem("parex", "JSC bank \"Citadele\""));
                _systems.Add(new PaidThroughSystem("vb2", "JSC bank \"SEB\""));
                _systems.Add(new PaidThroughSystem("wallet", "Paysera account"));
                _systems.Add(new PaidThroughSystem("webmoney", "International \"WebMoney\" system"));
                _systems.Add(new PaidThroughSystem("lt_mokilizingas", "Mokilizingas"));
                _systems.Add(new PaidThroughSystem("lt_post", "\"Paypost\" kiosks and Lithuanian post offices"));
                _systems.Add(new PaidThroughSystem("lt_perlas", "In \"Perlas\" lottery terminals"));
                _systems.Add(new PaidThroughSystem("worldhand", "International payment in Euros"));
                _systems.Add(new PaidThroughSystem("lt_gf_leasing", "General Financing"));
                _systems.Add(new PaidThroughSystem("sving", "Sving system"));
                _systems.Add(new PaidThroughSystem("maximalt", "MAXIMA Lietuva"));
                _systems.Add(new PaidThroughSystem("barcode", "\"Lietuvos spauda\" and \"Narvesen\" kiosks"));
                _systems.Add(new PaidThroughSystem("lthand", "Pay by cash"));
                _systems.Add(new PaidThroughSystem("lv_lpb", "Credit cards"));
                _systems.Add(new PaidThroughSystem("1stpay", "1stPayments"));
            }

            return _systems;
        }

        public PaidThroughSystem GetPaidThroughSystemByCode(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return null;
            }
            return GetPaidThroughSystems().FirstOrDefault(x => x.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public string GetPaidThroughSystemName(string code)
        {
            var s = GetPaidThroughSystemByCode(code);
            return s == null ? code : s.Name;
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
