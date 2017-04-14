using GiftService.Models;
using GiftService.Models.Auth;
using GiftService.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IAuthBll
    {
        User WebLogin(string email, string password);
    }

    public class AuthBll : IAuthBll
    {
        private IValidationBll _validationBll = null;

        private IEnumerable<User> _users;
        private IEnumerable<RoleModel> _roles;
        private IEnumerable<PosBdo> _pos;

        public AuthBll(IValidationBll validationBll)
        {
            _validationBll = validationBll ?? throw new ArgumentNullException(nameof(validationBll));

            #region Fake users

            _roles = new List<RoleModel>
            {
                new RoleModel { Id = "5b2f1a83-d177-4d09-a6c0-2e92e1136b4e", Name = "Administrator", Selected = false },
                new RoleModel { Id = "95443685-91e0-4436-990f-262e847e5817", Name = "User", Selected = false },
                new RoleModel { Id = "362f8f0d-f58f-455b-831c-fe31b1f73bb5", Name = "POS administrator", Selected = false }
            };
            var developerRoles = new List<RoleModel>(_roles);
            developerRoles.Add(new RoleModel { Id = "53f954a4-04e7-4f84-8a7c-c5c39295953d", Name = "Developer", Selected = true });

            var posRoles = new List<RoleModel> { _roles.ToArray()[1], _roles.ToArray()[2] };
            foreach (var r in posRoles)
            {
                r.Selected = true;
            }

            var adminRoles = _roles;
            foreach (var r in adminRoles)
            {
                r.Selected = true;
            }

            _users = new List<User>
            {
                new User
                {
                    Email = "proglamer@gmail.com", Password = "Qaz12345",
                    UserId = "4a84fee3-6318-44fc-867e-5c1488717e35", IsLocked = false, Username = "DK admin",
                    Roles = developerRoles
                },
                new User
                {
                    Email = "henrika.vind@gmail.com", Password = "Qaz12345",
                    UserId = "7d153bb6-8139-453a-a931-0f1e79317fd1", IsLocked = false, Username = "Henrika (admin)",
                    Roles = adminRoles
                },
                new User
                {
                    Email = "vida.juodv@gmail.com", Password = "Qaz12345",
                    UserId = "4d9819f1-fc66-4315-8515-8d29d4ffe93f", IsLocked = false, Username = "Vida (Knygynai.lt)",
                    Roles = posRoles
                },
                new User
                {
                    Email = "ritazibutiene@gmail.com", Password = "Qaz12345",
                    UserId = "833b1800-fe86-4fc1-92f7-022d2f003793", IsLocked = false, Username = "Rita (RitosMasazai.lt)",
                    Roles = posRoles
                },
                new User
                {
                    Email = "melisanda@info.lt", Password = "Qaz12345",
                    UserId = "e20c60a5-13a2-49c3-ace9-b0135d34814f", IsLocked = false, Username = "Melisanda.lt",
                    Roles = posRoles
                },
                new User
                {
                    Email = "prog.lamer@gmail.com", Password = "Qaz12345",
                    UserId = "24bfd99c-4eb0-4921-a6b5-9cf1bb2cbafa", IsLocked = false, Username = "Simple user",
                    Roles = adminRoles.Where(x => x.Name.StartsWith("User"))
                }
            };

            #endregion
        }

        public User WebLogin(string email, string password)
        {
            var errors = new List<ValidationError>();

            if (_validationBll.IsEmailValid(email, true, errors) == false)
            {
                throw new ValidationListException("E-mail is incorrect", errors);
            }

            var u = _users.First(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && x.Password.Equals(password));

            return u;
        }
    }
}
