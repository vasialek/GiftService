using GiftService.Models;
using GiftService.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{

    public interface ISecurityBll
    {
        void ValidatePosPaymentRequest(PosBdo pos);
    }

    public class SecurityBll : ISecurityBll
    {
        private ICommunicationBll _communicationBll = null;

        public SecurityBll(ICommunicationBll communicationBll)
        {
            if (communicationBll == null)
            {
                throw new ArgumentNullException("communicationBll");
            }

            _communicationBll = communicationBll;
        }

        public void ValidatePosPaymentRequest(PosBdo pos)
        {
            if (pos.ValidateUrl == null)
            {
                throw new ArgumentNullException("POS does not have ValidateUrl to validate request from it");
            }

            try
            {
                var response = _communicationBll.GetJsonResponse<BaseResponse>(pos.ValidateUrl);
                if (response == null)
                {
                    throw new Models.Exceptions.BadResponseException("Got NULL response from POS on request validation");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
