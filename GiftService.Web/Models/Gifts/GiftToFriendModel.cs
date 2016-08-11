using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiftService.Web.Models.Gifts
{
    public class GiftToFriendModel
    {
        public string ProductUid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Label_RequiredMale", ErrorMessageResourceType = typeof(Resources.Language))]
        public string FriendEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Label_RequiredMale", ErrorMessageResourceType = typeof(Resources.Language))]
        public string Text { get; set; }

        public string BtnEmailGift { get; set; }

        public string BtnDownloadGift { get; set; }
    }
}
