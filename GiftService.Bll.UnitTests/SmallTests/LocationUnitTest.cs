using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models.Products;

namespace GiftService.Bll.UnitTests.SmallTests
{
    [TestClass]
    public class LocationUnitTest
    {
        [TestMethod]
        public void Test_NameAddress_Complete()
        {
            var l = new ProductServiceLocation { Name = "Test shop", City = "Vilniu", Address = "Zakulicki 13" };
            string expected = String.Format("{0}. {1}, {2}", l.Name, l.Address, l.City);

            Assert.AreEqual(expected, l.NameAddress);
        }

        [TestMethod]
        public void Test_NameAddress_Without_Name()
        {
            var l = new ProductServiceLocation { Name = "", City = "Vilniu", Address = "Zakulicki 13" };
            string expected = String.Format("{0}, {1}", l.Address, l.City);

            Assert.AreEqual(expected, l.NameAddress);
        }
    }
}
