using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Auth
{
    public class User
    {
        public string UserId { get; set; }

        public bool IsLocked { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; } = new RoleModel[0];

        /// <summary>
        /// List of POS user owns
        /// </summary>
        public IEnumerable<PosBdo> MyPos { get; set; } = new PosBdo[0];
    }
}
