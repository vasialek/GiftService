using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{
    public class ContactUsModel
    {
        public bool IsSent { get; set; }

        [Display(Name = "Tema")]
        public string Subject { get; set; }

        [Display(Name = "Jūsų svetainės adresas")]
        public string SiteUrl { get; set; }

        [Display(Name = "Jūsų vardas")]
        public string Name { get; set; }

        [Display(Name = "Jūsų el. paštas")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessageResourceName = "Label_RequiredMale", ErrorMessageResourceType = typeof(Resources.Language))]
        public string Email { get; set; }

        [Display(Name = "Užklausa")]
        [Required(ErrorMessageResourceName = "Label_RequiredMale", ErrorMessageResourceType = typeof(Resources.Language))]
        [MinLength(20)]
        [MaxLength(1024)]
        public string Text { get; set; }

        public ContactUsModel()
        {
            IsSent = false;
        }
    }
}
