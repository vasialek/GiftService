using GiftService.Models.Payments;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GiftService.Bll
{
    public class PayseraBll
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

        public PayseraPaymentResponse ParseData(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data", "Response from payment system should not be empty");
            }

            int pos = data.IndexOf("data=", StringComparison.Ordinal);
            if (pos >= 0)
            {
                data = data.Substring(pos + "data=".Length);
            }

            data = data.Replace('-', '+')
                .Replace('_', '/');

            data = data.Replace("%3D", "=");
            byte[] ba = Convert.FromBase64String(data);
            //data = WebUtility.UrlDecode(data);
            string s = Encoding.UTF8.GetString(ba);

            // Got string like this: "orderid=bfaafc61900b456c87310cc8756d80ab&amount=1&currency=EUR&country=LT&test=1&payment=nordealt&p_email=it%40interateitis.lt&p_firstname=Aleksej&p_lastname=Vasinov&p_phone=%2B370+5+2166481&p_comment=Just+test+payment&p_ip=&p_agent=&p_file=&version=1.6&projectid=76457&lang=lit&paytext=U%C5%BEsakymas+nr%3A+bfaafc61900b456c87310cc8756d80ab+http%3A%2F%2Fwww.dovanukuponai.com+projekte.+%28Pardav%C4%97jas%3A+Rita+%C5%BDibutien%C4%97%29&m_pay_restored=90685289&status=1&requestid=90685289&payamount=1&paycurrency=EUR"
            var pars = HttpUtility.ParseQueryString(s);

            return BuildResponseFromHttpParameters(pars);
        }

        public PayseraPaymentResponse BuildResponseFromHttpParameters(NameValueCollection pars)
        {
            PayseraPaymentResponse resp = new PayseraPaymentResponse();

            foreach (string key in pars.AllKeys)
            {
                switch (key)
                {
                    case "status":
                        resp.Status = pars[key];
                        break;
                    case "orderid":
                        resp.OrderId = pars[key];
                        break;
                    case "projectid":
                        resp.ProjectId = pars[key];
                        break;
                    case "test":
                        resp.IsTestPayment = pars[key] == "0" ? false : true;
                        break;
                    case "amount":
                        resp.AmountToPay = GetDecimal(pars[key], -1m);
                        if (resp.AmountToPay > 0)
                        {
                            resp.AmountToPay /= 100m;
                        }
                        break;
                    case "payamount":
                        resp.PayAmount = GetDecimal(pars[key], -1m);
                        if (resp.PayAmount > 0)
                        {
                            resp.PayAmount /= 100m;
                        }
                        break;
                    case "currency":
                        resp.CurrencyCode = pars[key];
                        break;
                    case "paycurrency":
                        resp.PayCurrencyCode = pars[key];
                        break;
                    case "p_firstname":
                        resp.CustomerName = pars[key];
                        break;
                    case "p_lastname":
                        resp.CustomerLastName = pars[key];
                        break;
                    case "p_email":
                        resp.CustomerEmail = pars[key];
                        break;
                    case "p_phone":
                        resp.CustomerPhone = pars[key];
                        break;
                    case "p_comment":
                        resp.Remarks = pars[key];
                        break;
                    case "paytext":
                        resp.PayText = pars[key];
                        break;
                    case "payment":
                        resp.Payment = pars[key];
                        break;
                    default:
                        break;
                }
            }

            return resp;
        }

        protected decimal GetDecimal(string s, decimal defaultValue)
        {
            decimal d;
            if (decimal.TryParse(s, out d) == false)
            {
                d = defaultValue;
            }
            return d;
        }

        public Uri PreparePaymentLink(Uri payserPaymentUrl, PayseraPaymentRequest rq)
        {
            Logger.Info("Preparing payment link for Payser");
            ValidatePaymentRequest(rq);

            string data = EncodeString(JoinPaymentParameters(rq));
            Logger.Info("  data=" + data);

            string ss1Sign = GenerateSs1(data, rq.PayseraProjectPassword);
            Logger.Info("  ss1: " + ss1Sign);

            // Need to convert '=' to "%3D" (Paysera)
            data.Replace("=", "%3D");
            return new Uri(payserPaymentUrl, String.Concat("?data=", data, "&sign=", ss1Sign));
        }

        public string GenerateSs1(string data, string payseraProjectPassword)
        {
            string s = String.Concat(data, payseraProjectPassword);

            byte[] ba = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(s));

            // To lower is nNecessary for Paysera
            return BllFactory.Current.HelperBll.EncodeHex(ba).ToLower();
        }

        /// <summary>
        /// Throws exception when any of mandatory fields is absent
        /// </summary>
        public void ValidatePaymentRequest(PayseraPaymentRequest rq)
        {
            if (String.IsNullOrEmpty(rq.ProjectId))
            {
                throw new ArgumentNullException("ProjectId");
            }

            if (String.IsNullOrEmpty(rq.OrderId))
            {
                throw new ArgumentNullException("OrderId");
            }

            if (rq.AcceptUrl == null)
            {
                throw new ArgumentNullException("AcceptUrl");
            }

            if (rq.CancelUrl == null)
            {
                throw new ArgumentNullException("CancelUrl");
            }

            if (rq.CallbackUrl == null)
            {
                throw new ArgumentNullException("CallbackUrl");
            }

            //if (String.IsNullOrEmpty(rq.Version))
            //{
            //    throw new ArgumentNullException("Version");
            //}
        }

        public string EncodeString(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Could not encode empty string to send to Paysera");
            }

            s = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));

            // Replace '/' => '_', '+' => '-'
            return s.Replace('/', '_')
                .Replace('+', '-');
        }

        /// <summary>
        /// Joins all request parameters to one string (with UrlEncode)
        /// </summary>
        public string JoinPaymentParameters(PayseraPaymentRequest rq)
        {
            Dictionary<string, string> ar = new Dictionary<string, string>();
            ar["orderid"] = rq.OrderId;
            ar["amount"] = (rq.AmountToPay * 100).ToString("#");
            ar["currency"] = rq.CurrencyCode;
            ar["country"] = rq.Country.ToString();
            ar["accepturl"] = rq.AcceptUrl;
            ar["cancelurl"] = rq.CancelUrl;
            ar["callbackurl"] = rq.CallbackUrl;
            ar["test"] = rq.IsTestPayment ? "1" : "0";
            ar["payment"] = rq.RequestedPaymentMehtods;
            ar["p_email"] = rq.CustomerEmail;
            ar["p_firstname"] = rq.CustomerName;
            ar["p_lastname"] = rq.CustomerLastName;
            ar["p_phone"] = rq.CustomerPhone;
            ar["p_comment"] = rq.Remarks;
            ar["p_ip"] = rq.Ip;
            ar["p_agent"] = rq.BrowserAgent;
            ar["p_file"] = "";
            ar["version"] = PayseraPaymentRequest.Version;
            ar["projectid"] = rq.ProjectId;

            //string[] p = ar.Select(x => String.Concat(x.Key, "=", WebUtility.UrlEncode(x.Value))).ToArray();
            string[] p = ar.Select(x => String.Concat(x.Key, "=", UrlEncode(x.Value))).ToArray();

            return String.Join("&", p);
        }

        public string UrlEncode(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return "";
            }
            s = WebUtility.UrlEncode(s);

            // Replace ( => %28, ) => %29 to make same as PHP version of Paysera
            return s.Replace("(", "%28")
                .Replace(")", "%29");
        }
    }
}
