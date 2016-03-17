using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Dal;
using Moq;
using GiftService.Models;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class PosBllUnitTest
    {
        private Mock<IConfigurationBll> _configBll = new Mock<IConfigurationBll>();
        private IPosBll _posBll = null;

        [TestInitialize]
        public void Init()
        {
            _configBll.Setup(x => x.Get())
                .Returns(new Models.MySettings { LengthOfPosUid = 32 });

            _posBll = new PosBll(_configBll.Object, DalFactory.Current.PosDal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Extract_PosId_From_Bad_Uid()
        {
            string uidFromPos = "0";

            int posId = _posBll.GetPosIdFromUid(uidFromPos);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_Extract_PosId_From_Incorrect_Uid()
        {
            string uidFromPost = "0".PadLeft(_configBll.Object.Get().LengthOfPosUid, '0');

            int posId = _posBll.GetPosIdFromUid(uidFromPost);
        }

        [TestMethod]
        public void Test_Extract_PosId_From_Uid()
        {
            char[] uidFromPos = "1234567890123456".PadRight(_configBll.Object.Get().LengthOfPosUid, '0').ToCharArray();
            uidFromPos[1] = '1';
            uidFromPos[4] = '0';
            uidFromPos[7] = '0';
            uidFromPos[10] = '5';

            int posId = _posBll.GetPosIdFromUid(new String(uidFromPos));

            Assert.AreEqual(1005, posId);
        }

        #region Payment note format

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Error_On_No_Product_Formatting_PaymentNote()
        {
            ProductBdo p = null;

            _posBll.FormatNoteForPayment(null, p, 250);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Error_On_Null_ProductName()
        {
            ProductBdo p = new ProductBdo();

            _posBll.FormatNoteForPayment(null, p, 250);
        }

        [TestMethod]
        public void Test_Form_PaymentNote_Is_Not_Empty()
        {
            PosBdo pos = new PosBdo();
            ProductBdo product = new ProductBdo { ProductName = "Product" };

            string note = _posBll.FormatNoteForPayment(pos, product, 250);

            Assert.IsFalse(String.IsNullOrEmpty(note));
        }

        [TestMethod]
        public void Test_PaymentNote_Contains_Pos_Name()
        {
            PosBdo pos = new PosBdo { Name = "MyShop" };
            ProductBdo product = new ProductBdo { ProductName = "Product" };

            string note = _posBll.FormatNoteForPayment(pos, product, 250);

            Assert.IsTrue(note.IndexOf(pos.Name) >= 0);
        }

        [TestMethod]
        public void Test_PaymentNote_Must_Contain_OrderNr()
        {
            PosBdo pos = new PosBdo();
            ProductBdo product = new ProductBdo { ProductName = "Product" };

            string note = _posBll.FormatNoteForPayment(pos, product, 250);

            Assert.IsTrue(note.IndexOf("[order_nr]") >= 0);
        }

        [TestMethod]
        public void Test_PaymentNote_Must_Contains_OwnerName()
        {
            PosBdo pos = new PosBdo { Id = 1005, Name = "MyShop" };
            ProductBdo product = new ProductBdo { ProductName = "Product" };

            string note = _posBll.FormatNoteForPayment(pos, product, 250);

            Assert.IsTrue(note.IndexOf("[owner_name]") > 0);
        }

        [TestMethod]
        public void Test_PaymentNote_Does_Not_Exceed_MaxLength()
        {
            int maxLength = 120;
            PosBdo pos = new PosBdo { Id = 1005, Name = "My super-puper online e-Shop" };
            ProductBdo product = new ProductBdo { ProductName = "product very long-long-long name" };

            string note = _posBll.FormatNoteForPayment(pos, product, maxLength);

            Assert.IsTrue(note.Length <= maxLength);
        }

        [TestMethod]
        public void Test_Extremely_Long_Name_For_PaymentNote()
        {
            PosBdo pos = new PosBdo { Id = 1005, Name = "My super-puper online e-Shop" };
            ProductBdo product = new ProductBdo { ProductName = "product very long-long-long-long-long name" };
            int maxLength = pos.Name.Length + product.ProductName.Length;

            // Check no exception
            string note = _posBll.FormatNoteForPayment(pos, product, maxLength);
        }

        #endregion
    }
}
