using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Dal;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class PosBllUnitTest
    {
        private IPosBll _posBll = new PosBll(DalFactory.Current.PosDal);

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
            string uidFromPost = "0".PadLeft(16, '0');

            int posId = _posBll.GetPosIdFromUid(uidFromPost);
        }

        [TestMethod]
        public void Test_Extract_PosId_From_Uid()
        {
            char[] uidFromPos = "1234567890123456".ToCharArray();
            uidFromPos[1] = '1';
            uidFromPos[4] = '0';
            uidFromPos[7] = '0';
            uidFromPos[10] = '5';

            int posId = _posBll.GetPosIdFromUid(new String(uidFromPos));

            Assert.AreEqual(1005, posId);
        }
    }
}
