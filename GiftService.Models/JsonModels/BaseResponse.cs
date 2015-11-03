using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.JsonModels
{
    public class BaseResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }

        public BaseResponse()
        {
            Errors = new string[0];
        }
    }
}
