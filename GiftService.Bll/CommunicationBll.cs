using GiftService.Models;
using GiftService.Models.Exceptions;
using GiftService.Models.JsonModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface ICommunicationBll
    {
        T GetJsonResponse<T>(Uri jsonUrl) where T : BaseResponse;
        T ParseJson<T>(string json) where T : BaseResponse;
        void SendEmailToClientOnSuccess(ProductBdo product);
        void SendEmailToManager(string subject, string body);
        void SendCouponAsGift(string friendEmail, byte[] pdf, ProductInformationModel product);
    }

    public class FakeCommunicationBll : ICommunicationBll
    {
        public T GetJsonResponse<T>(Uri jsonUrl) where T : BaseResponse
        {
            string json = "{\"Version\":\"C\",\"Status\":\"true\",\"Message\":\"\",\"ResponseCode\":666,\"PosId\":0,\"ProductName\":\"Plauk\u0173 atstatomoji proced\u016bra Hialurono 50cm\",\"ProductDuration\":\"\",\"ProductDescription\":\"\",\"RequestedAmountMinor\":\"2000\",\"CurrencyCode\":\"EUR\",\"ProductValidTillTm\":1497502800,\"PosName\":\"\",\"PosUrl\":\"\",\"PosCity\":\"\",\"PosAddress\":\"\",\"PhoneForReservatio \":\"\",\"EmailForReservation\":\"\",\"Locations\":[{\"Id\":1,\"Name\":\"\",\"City\":\"Vilnius\",\"Address\":\"Kalvarij\u0173 g. 119, Apkas\u0173 g. 2\",\"LatLng\":\"\",\"EmailReservation\":\"\",\"PhoneReservation\":\"+370 630 06009\"}]}";

            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public T ParseJson<T>(string json) where T : BaseResponse
        {
            throw new NotImplementedException();
        }

        public void SendCouponAsGift(string friendEmail, byte[] pdf, ProductInformationModel product)
        {
            throw new NotImplementedException();
        }

        public void SendEmailToClientOnSuccess(ProductBdo product)
        {
            throw new NotImplementedException();
        }

        public void SendEmailToManager(string subject, string body)
        {
            throw new NotImplementedException();
        }
    }

    public class CommunicationBll : ICommunicationBll
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

        private IConfigurationBll _configurationBll = null;

        public CommunicationBll(IConfigurationBll configurationBll)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }

            _configurationBll = configurationBll;
        }

        public T GetJsonResponse<T>(Uri jsonUrl) where T : BaseResponse
        {
            if (jsonUrl == null)
            {
                throw new ArgumentNullException("jsonUrl", "Should be valid URL to load JSON");
            }

            WebClient wc = new WebClient();
            byte[] ba = wc.UploadData(jsonUrl, "POST", new byte[0]);
            if (ba == null || ba.Length == 0)
            {
                throw new BadResponseException("Validate URL returns NULL response from POS");
            }

            string resp = "";
            resp = Encoding.UTF8.GetString(ba);
            Logger.Debug("Got payment request validation request from POS: " + resp);
            //resp = wc.DownloadString(jsonUrl);

            return ParseJson<T>(resp);
        }

        public T ParseJson<T>(string json) where T : BaseResponse
        {
            T r = default(T);

            try
            {
                r = (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                return r;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SendCouponAsGift(string friendEmail, byte[] pdf, ProductInformationModel product)
        {
            if (pdf == null)
            {
                throw new ArgumentNullException("pdf");
            }

            try
            {
                Logger.Info("Sending email with gift coupon to friend: " + friendEmail);

                var ms = _configurationBll.Get();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("webmaster@DovanuKuponai.com", "DovanuKuponai");
                //mailMessage.To.Add(new MailAddress(ms.Mails.ManagerEmail, "DK manager"));
                mailMessage.To.Add(new MailAddress(friendEmail));
                mailMessage.Bcc.Add(new MailAddress("proglamer@gmail.com"));
                mailMessage.Subject = String.Concat("Draugo dovana is ", ms.ProjectName);
                mailMessage.Body = "Jusu draugas siuncia Jums dovanu kupona. " + ms.ProjectName;

                using (var s = new System.IO.MemoryStream(pdf))
                {
                    Attachment a = null;
                    a = new Attachment(s, "DovanuKuponas.pdf", "application/pdf");
                    mailMessage.Attachments.Add(a);
                    mailMessage.IsBodyHtml = false;

                    SendEmail(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error sending coupon as gift to: " + friendEmail, ex);
                throw;
            }
        }

        public void SendEmailToManager(string subject, string body)
        {
            try
            {
                Logger.Info("Sending email to manager");

                var ms = _configurationBll.Get();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("webmaster@DovanuKuponai.com", "DovanuKuponai");
                mailMessage.To.Add(new MailAddress(ms.Mails.ManagerEmail, "DK manager"));
                //mailMessage.To.Add(new MailAddress("proglamer@gmail.com"));
                mailMessage.Bcc.Add(new MailAddress("proglamer@gmail.com"));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                SendEmail(mailMessage);
            }
            catch (Exception ex)
            {
                Logger.Error("Error sending e-mail to manager", ex);
                throw;
            }
        }

        public void SendEmailToClientOnSuccess(ProductBdo product)
        {
            try
            {
                Logger.Info("Sending email to client on success payment");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("RitosMasazai@DovanuKuponai.com", "Ritos Masazai");
                mailMessage.To.Add(new MailAddress(product.CustomerEmail, product.CustomerName));
                //mailMessage.To.Add(new MailAddress("proglamer@gmail.com"));
                mailMessage.Bcc.Add(new MailAddress("proglamer@gmail.com"));
                mailMessage.Subject = "RitosMasazai.lt - Kuponas " + product.ProductName;
                mailMessage.Body = FormatClientEmail(product);
                mailMessage.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("mail.dovanukuponai.com");
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("RitosMasazai@dovanukuponai.com", "7sSVYyT_8Wpz");
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Logger.Error("Error sending e-mail to client on success payment", ex);
                throw;
            }
        }

        public string FormatClientEmail(ProductBdo product)
        {
            StringBuilder sb = new StringBuilder(@"Mielas kliente!

Jūs įsigijote kuponą www.ritosmasazai.lt
%CustomerName%
%ProductName%
%ProductDuration% %ProductPrice% %CurrencyCode%
Privalote užsiregistruoti telefonu:
8652 98422
Salonas: 
%PosName%
%PosAddress%, %PosCity%
* Kuponas galioja iki %ValidTill%

<a href=""http://www.dovanukuponai.com/gift/get/%PaySystemUid%"">Daugiau informacijos</a>
Ačiū, kad pirkote!");

            sb.Replace("%CustomerName%", product.CustomerName);
            sb.Replace("%ProductName%", product.ProductName);
            //sb.Replace("%ProductDuration%", product.Du);
            sb.Replace("%ProductPrice%", product.ProductPrice.ToString("### ##0.00"));
            sb.Replace("%CurrencyCode%", product.CurrencyCode);
            sb.Replace("%PosName%", product.PosName);
            sb.Replace("%PosAddress%", product.PosAddress);
            sb.Replace("%PosCity%", product.PosCity);
            //sb.Replace("%ProductUid%", product.ProductUid);
            sb.Replace("%PaySystemUid%", product.PaySystemUid);
            sb.Replace("%ValidTill%", product.ValidTill.ToShortDateString());

            sb.Replace(Environment.NewLine, "<br />");

            return sb.ToString();
        }

        protected void SendEmail(MailMessage mailMessage)
        {
            SmtpClient client = new SmtpClient("mail.dovanukuponai.com");
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("RitosMasazai@dovanukuponai.com", "7sSVYyT_8Wpz");
            client.Credentials = new NetworkCredential("webmaster@dovanukuponai.com", "DovanKup_Wmaster29");
            client.Send(mailMessage);
        }
    }
}

