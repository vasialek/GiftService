using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models.Auth;
using System.Collections.Generic;
using System.IO;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class DumpBllUnitTest
    {
        private IList<RoleModel> _roles = null;
        private User _user = null;

        [TestInitialize]
        public void Init()
        {
            _roles = new List<RoleModel>
            {
                new RoleModel { Id = "5b2f1a83-d177-4d09-a6c0-2e92e1136b4e", Name = "Administrator", Selected = true },
                new RoleModel { Id = "95443685-91e0-4436-990f-262e847e5817", Name = "User", Selected = false },
                new RoleModel { Id = "362f8f0d-f58f-455b-831c-fe31b1f73bb5", Name = "POS administrator", Selected = false }
            };
            _user = new User
            {
                Email = "proglamer@gmail.com",
                Password = "123456",
                UserId = "4a84fee3-6318-44fc-867e-5c1488717e35",
                IsLocked = false,
                Username = "DK admin",
                Roles = _roles
            };
        }

        [TestMethod]
        public void Test_Dump_ProductBdo()
        {
            string s = DumpBll.Dump(new Models.ProductBdo());
            Assert.AreEqual("-", s);
        }

        [TestMethod]
        public void Dump_User_Roles()
        {
            string s = DumpBll.Dump(_user.Roles, "Some user roles:");

            File.WriteAllText("c:\\temp\\_dumproles.txt", s);
        }
    }
}
