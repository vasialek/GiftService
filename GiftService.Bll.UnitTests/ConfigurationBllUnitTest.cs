using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class ConfigurationBllUnitTest
    {
        private IConfigurationBll _bll = new ConfigurationBll();


        [TestMethod]
        public void Test_GetDirectoryNameByUid_Success()
        {
            string uid = Guid.NewGuid().ToString("N");
            int dirNameLength = _bll.Get().LengthOfPdfDirectoryName;

            string dirName = _bll.GetDirectoryNameByUid(uid);

            Assert.AreEqual(dirNameLength, dirName.Length);
            Assert.AreEqual(uid.Substring(0, dirNameLength), dirName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_GetDirectoryNameByUid_Empty()
        {
            string uid = "";

            _bll.GetDirectoryNameByUid(uid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetDirectoryNameByUid_Bad_Length()
        {
            string uid = "a".PadLeft(_bll.Get().LengthOfPosUid + 1, 'b');

            _bll.GetDirectoryNameByUid(uid);
        }
    }
}
