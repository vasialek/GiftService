using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models;
using System.Drawing;
using System.Linq;
using GiftService.Models.Exceptions;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class HelperBllUnitTest
    {
        private IHelperBll _bll = new HelperBll();

        [TestMethod]
        public void Test_Convert_Zero_As_Unix_Begin()
        {
            uint tm = 0;

            DateTime d = _bll.ConvertFromUnixTimestamp(tm);

            Assert.AreEqual(_bll.UnixStart, d);
        }

        [TestMethod]
        public void Test_Get_Current_Timestamp()
        {
            uint tm = _bll.GetUnixTimestamp();
            DateTime dt = _bll.ConvertFromUnixTimestamp(tm);

            Assert.IsTrue((DateTime.UtcNow - dt).TotalSeconds < 1);
        }

        [TestMethod]
        public void Test_Ymap_Vilnius_Center()
        {
            // 54.688229, 25.267051
            var ll = new LatLng { Lat = 54.688229, Lng = 25.267051 };

            string s = _bll.GetLatLngString(ll, MapTypes.YandexMap);

            Assert.AreEqual("fsadfdsfsdfsdfsd", s);
        }

        [TestMethod]
        public void Test_Gmap_Vilnius_Center()
        {
            // 54.688229, 25.267051
            var ll = new LatLng { Lat = 54.688229, Lng = 25.267051 };

            string s = _bll.GetLatLngString(ll, MapTypes.GoogleMap);

            Assert.AreEqual("fsadfdsfsdfsdfsd", s);
        }

        [TestMethod]
        public void Test_GetLatLng_Check_Format()
        {
            var ll = new LatLng { Lat = 1, Lng = 2 };

            string s = _bll.GetLatLngString(ll);

            Assert.AreEqual("1.000000,2.000000".Length, s.Length);

        }

        [TestMethod]
        public void Test_ParseLatLng_Gmap_With_Slash()
        {
            string s = "54.69212/25.28487";

            var ll = _bll.ParseLatLng(s, MapTypes.GoogleMap);

            Assert.IsNotNull(ll);
            Assert.AreEqual(54.69212, ll.Lat);
            Assert.AreEqual(25.28487, ll.Lng);
        }

        [TestMethod]
        public void Test_ParseLatLng_From_Gmap_To_Ymap()
        {
            string s = "54.69212/25.28487";

            var ll = _bll.ParseLatLng(s, MapTypes.GoogleMap);
            string sMap = _bll.GetLatLngString(ll, MapTypes.YandexMap);

            System.Diagnostics.Process.Start("chrome.exe", "http://static-maps.yandex.ru/1.x/?lang=lt&ll=" + sMap + "&size=500,450&z=15&l=map&pt=" + sMap + ",flag");
        }

        #region Full URL helpers

        [TestMethod]
        public void Get_Full_Url_No_Culture_No_Ssl()
        {
            BllFactory.Current.ConfigurationBll.Get().WebOptions.UseSsl = false;
            BllFactory.Current.ConfigurationBll.Get().ProjectDomain = "www.DovanuKuponai.com";

            string url = _bll.GetFullUrl();

            Assert.AreEqual("http://www.dovanukuponai.com/", url);
        }

        [TestMethod]
        public void Get_Full_Url_No_Culture_Sll()
        {
            BllFactory.Current.ConfigurationBll.Get().WebOptions.UseSsl = true;
            BllFactory.Current.ConfigurationBll.Get().ProjectDomain = "www.DovanuKuponai.com";

            string url = _bll.GetFullUrl();

            Assert.AreEqual("https://www.dovanukuponai.com/", url);
        }

        [TestMethod]
        public void Get_Full_Url_Lt_No_Ssl()
        {
            string webCulture = "lt";
            BllFactory.Current.ConfigurationBll.Get().WebOptions.UseSsl = false;
            BllFactory.Current.ConfigurationBll.Get().ProjectDomain = "www.DovanuKuponai.com";

            string url = _bll.GetFullUrl(webCulture);

            Assert.AreEqual(String.Concat("http://www.dovanukuponai.com/", webCulture, "/"), url);
        }

        [TestMethod]
        public void Get_Full_Url_En_Ssl_Http()
        {
            BllFactory.Current.ConfigurationBll.Get().ProjectDomain = "https://www.dovanukuponai.com";

            string url = _bll.GetFullUrl("en");

            Assert.AreEqual("https://www.dovanukuponai.com/en/", url);
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_On_Incorrect_Product_Uid()
        {
            var p = new ProductBdo();

            _bll.GetProductStatusQr(p, 10, "lt");
        }

        [TestMethod]
        public void Generate_Qr_Code()
        {
            var p = Fakes.ProductsDalFake.GetProducts().First();
            string filename = "c:\\temp\\empty_qr.png";

            Bitmap bmp = _bll.GetProductStatusQr(p, 20, "lt");

            bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);

            System.Diagnostics.Process.Start("explorer.exe", filename);
        }
    }
}
