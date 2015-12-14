using GiftService.Models;
using GiftService.Models.Exceptions;
using GiftService.Models.JsonModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{

    public interface ISecurityBll
    {
        PaymentRequestValidationResponse ValidatePosPaymentRequest(PosBdo pos, string posUserUid);
        void ValidateCurrencyCode(string currencyCode, PosBdo pos);
        void ValidateUid(string uid);
    }

    public class SecurityBll : ISecurityBll
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

        private ICommunicationBll _communicationBll = null;
        private IConfigurationBll _configurationBll = null;

        public SecurityBll(IConfigurationBll configurationBll, ICommunicationBll communicationBll)
        {
            if (communicationBll == null)
            {
                throw new ArgumentNullException("communicationBll");
            }
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }

            _communicationBll = communicationBll;
            _configurationBll = configurationBll;
        }

        public void ValidateCurrencyCode(string currencyCode, PosBdo pos)
        {
            if (String.IsNullOrEmpty(currencyCode))
            {
                throw new IncorrectPaymentParamersException("Payment request must specify currency code");
            }

        }

        public PaymentRequestValidationResponse ValidatePosPaymentRequest(PosBdo pos, string posUserUid)
        {
            if (pos.ValidateUrl == null)
            {
                throw new ArgumentNullException("POS does not have ValidateUrl to validate request from it");
            }

            PaymentRequestValidationResponse response = null;

            try
            {
                var validationUri = new Uri(pos.ValidateUrl.ToString() + posUserUid);
                Logger.InfoFormat("Validating request payment from POS: `{0}`", validationUri.ToString());
                response = _communicationBll.GetJsonResponse<PaymentRequestValidationResponse>(validationUri);
                if (response == null)
                {
                    throw new BadResponseException("Got NULL response from POS on request validation");
                }

                if (response.RequestedAmountMinor < 1)
                {
                    throw new IncorrectPaymentParamersException("Payment request amount (minor) should be greater than 0");
                }

                if (String.IsNullOrEmpty(response.ProductName))
                {
                    throw new IncorrectPaymentParamersException("Payment request must specify product name");
                }

                ValidateCurrencyCode(response.CurrencyCode, pos);

                return response;
            }
            catch (InvalidCastException icex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Throws exception on NULL or empty string either UID is not exactly same length as in configuration
        /// </summary>
        public void ValidateUid(string uid)
        {
            if (String.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("UID should not be empty");
            }

            int len = _configurationBll.Get().LengthOfPosUid;
            if (uid.Length != len)
            {
                throw new ArgumentOutOfRangeException("uid", "Length of UID should be exactly: " + len);
            }
        }
    }
}
