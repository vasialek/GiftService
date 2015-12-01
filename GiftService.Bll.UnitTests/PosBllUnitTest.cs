using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Dal;
using Moq;

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
    }
}
