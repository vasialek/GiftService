using GiftService.Models.Exceptions;
using GiftService.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface ICommunicationBll
    {
        T GetJsonResponse<T>(Uri jsonUrl) where T : BaseResponse;
        T ParseJson<T>(string json) where T : BaseResponse;
    }

    public class CommunicationBll : ICommunicationBll
    {
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
            //resp = wc.DownloadString(jsonUrl);

            return ParseJson<T>(resp);
        }

        public T ParseJson<T>(string json) where T : BaseResponse
        {
            T r = default(T);

            try
            {
                r = (T)Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse>(json);
                return r;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

