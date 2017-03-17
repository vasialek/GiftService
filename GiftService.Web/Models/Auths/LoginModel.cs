using System.ComponentModel.DataAnnotations;

namespace GiftService.Web.Models.Auths
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

    }
}