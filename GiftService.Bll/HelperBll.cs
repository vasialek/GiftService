using GiftService.Models;
using GiftService.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using QRCoder;

namespace GiftService.Bll
{

    public interface IHelperBll
    {
        DateTime UnixStart { get; }
        DateTime ConvertFromUnixTimestamp(UInt32 timestamp);
        UInt32 GetUnixTimestamp();
        string EncodeHex(byte[] ba);
        string EncodeHex(byte[] ba, int offset, int length);
        byte[] DecodeHex(string s);
        string GenerateProductUid(int posId);
        Dictionary<string, object> DynamicObjectToDictionaryInsensitive(object o);
        Dictionary<string, object> DynamicObjectToDictionary(object o);

        string GetLatLngString(LatLng latLng);
        string GetLatLngString(LatLng latLng, MapTypes mapType);
        LatLng ParseLatLng(string s, MapTypes mapType);

        /// <summary>
        /// Generates simple QR code (black and white) to get product/service status - paid, used, invalid
        /// </summary>
        /// <param name="productBdo"></param>
        /// <param name="pixelsPerModule">33 (or 25) px * <paramref name="pixelsPerModule"/></param>
        /// <returns></returns>
        Bitmap GetProductStatusQr(ProductBdo productBdo, int pixelsPerModule, string webCultureName);

        /// <summary>
        /// Full URL to project => http(s)://www.dk.com/
        /// </summary>
        /// <returns></returns>
        string GetFullUrl(string webCultureName = null);
    }

    public class HelperBll : IHelperBll
    {
        private static char[] HX = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public LatLng ParseLatLng(string s, MapTypes mapType)
        {
            // Assume that coordinates are comma or slash separated. And decimal separator is dot
            string[] ar = s.Split(",/".ToCharArray());
            if (ar?.Length != 2)
            {
                throw new ValidationException("Expected 2 coordinates, separated with comma or slash. Error parsing incorrect string: " + s, ValidationErrors.NotEqual);
            }

            var ll = new LatLng();

            switch (mapType)
            {
                case MapTypes.YandexMap:
                    break;
                case MapTypes.GoogleMap:
                    // Expected lattiude/longtitude
                    ll.Lat = Double.Parse(ar[0], CultureInfo.InvariantCulture);
                    ll.Lng = Double.Parse(ar[1], CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ValidationException("Could not parse string to LatLng for map type: " + mapType.ToString(), ValidationErrors.IncorrectType);
            }

            return ll;
        }

        public string GetLatLngString(LatLng latLng)
        {
            return GetLatLngString(latLng, MapTypes.YandexMap);
        }

        public string GetLatLngString(LatLng latLng, MapTypes map)
        {
            if (latLng == null)
            {
                throw new ValidationException("LatLng object is NULL", ValidationErrors.Empty);
            }

            if (latLng.IsSet == false)
            {
                return "0.000000,0.000000";
            }

            double[] ar = new double[2];
            switch (map)
            {
                case MapTypes.YandexMap:
                    // Longitude and latitude in degrees;
                    ar[0] = latLng.Lng;
                    ar[1] = latLng.Lat;
                    break;
                case MapTypes.GoogleMap:
                    // comma-separated {latitude,longitude} pair
                    ar[0] = latLng.Lat;
                    ar[1] = latLng.Lng;
                    break;
                default:
                    throw new ValidationException("Could not convert LatLng to string for map: " + map.ToString(), ValidationErrors.IncorrectType);
            }

            return String.Concat(ar[0].ToString("0.000000", CultureInfo.InvariantCulture), ",", ar[1].ToString("0.000000", CultureInfo.InvariantCulture));
        }

        public Dictionary<string, object> DynamicObjectToDictionaryInsensitive(object o)
        {
            Dictionary<string, object> ar = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(o))
            {
                ar[pd.Name] = pd.GetValue(o);
            }
            return ar;
        }

        public Dictionary<string, object> DynamicObjectToDictionary(object o)
        {
            Dictionary<string, object> ar = new Dictionary<string, object>();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(o))
            {
                ar[pd.Name] = pd.GetValue(o);
            }
            return ar;
        }

        public DateTime UnixStart
        {
            get
            {
                return new DateTime(1970, 1, 1);
            }
        }

        public DateTime ConvertFromUnixTimestamp(uint timestamp)
        {
            return UnixStart.AddSeconds(timestamp);
        }

        public uint GetUnixTimestamp()
        {
            return (UInt32)((DateTime.UtcNow.Ticks - UnixStart.Ticks) / 10000000);
        }

        public string EncodeHex(byte[] ba)
        {
            if (ba == null)
            {
                return null;
            }

            char[] ca = new char[ba.Length << 1];
            for (int ck = 0; ck < ba.Length; ck++)
            {
                byte b = ba[ck];
                ca[(ck << 1)] = HX[b >> 4];
                ca[(ck << 1) + 1] = HX[b & 0x0F];
            }
            return new string(ca);
        }

        public string EncodeHex(byte[] ba, int offset, int length)
        {
            if (ba == null)
            {
                return null;
            }

            char[] ca = new char[length << 1];
            for (int ck = 0; ck < length; ck++)
            {
                byte b = ba[ck + offset];
                ca[(ck << 1)] = HX[b >> 4];
                ca[(ck << 1) + 1] = HX[b & 0x0F];
            }
            return new string(ca);
        }

        public byte[] DecodeHex(string s)
        {
            if (s == null) return null;
            byte[] ba = new byte[s.Length >> 1];
            int ck = 0;
            for (int i = 0; i < ba.Length; i++)
            {
                switch (s[ck++])
                {
                    case '0': break;
                    case '1': ba[i] = 0x10; break;
                    case '2': ba[i] = 0x20; break;
                    case '3': ba[i] = 0x30; break;
                    case '4': ba[i] = 0x40; break;
                    case '5': ba[i] = 0x50; break;
                    case '6': ba[i] = 0x60; break;
                    case '7': ba[i] = 0x70; break;
                    case '8': ba[i] = 0x80; break;
                    case '9': ba[i] = 0x90; break;
                    case 'A':
                    case 'a': ba[i] = 0xA0; break;
                    case 'B':
                    case 'b': ba[i] = 0xB0; break;
                    case 'C':
                    case 'c': ba[i] = 0xC0; break;
                    case 'D':
                    case 'd': ba[i] = 0xD0; break;
                    case 'E':
                    case 'e': ba[i] = 0xE0; break;
                    case 'F':
                    case 'f': ba[i] = 0xF0; break;
                    default: throw new ArgumentException("String is not hex encoded data.");
                }
                switch (s[ck++])
                {
                    case '0': break;
                    case '1': ba[i] |= 0x01; break;
                    case '2': ba[i] |= 0x02; break;
                    case '3': ba[i] |= 0x03; break;
                    case '4': ba[i] |= 0x04; break;
                    case '5': ba[i] |= 0x05; break;
                    case '6': ba[i] |= 0x06; break;
                    case '7': ba[i] |= 0x07; break;
                    case '8': ba[i] |= 0x08; break;
                    case '9': ba[i] |= 0x09; break;
                    case 'A':
                    case 'a': ba[i] |= 0x0A; break;
                    case 'B':
                    case 'b': ba[i] |= 0x0B; break;
                    case 'C':
                    case 'c': ba[i] |= 0x0C; break;
                    case 'D':
                    case 'd': ba[i] |= 0x0D; break;
                    case 'E':
                    case 'e': ba[i] |= 0x0E; break;
                    case 'F':
                    case 'f': ba[i] |= 0x0F; break;
                    default: throw new ArgumentException("String is not hex encoded data.");
                }
            }
            return ba;
        }

        public string GenerateProductUid(int posId)
        {
            if (posId < 1000 || posId > 9999)
            {
                throw new ArgumentOutOfRangeException("posId", "Could not generate product UID for such POS with ID: " + posId);
            }

            string s = posId.ToString();
            char[] uid = Guid.NewGuid().ToString("N").ToCharArray();
            uid[1] = s[3];
            uid[4] = s[2];
            uid[7] = s[1];
            uid[10] = s[0];

            return new string(uid);
        }

        public Bitmap GetProductStatusQr(ProductBdo product, int pixelsPerModule, string webCultureName)
        {
            BllFactory.Current.SecurityBll.ValidateUid(product.ProductUid);

            string url = String.Concat(GetFullUrl(webCultureName), "check/", product.ProductUid);

            var qrData = new QRCodeGenerator().CreateQrCode(url, QRCodeGenerator.ECCLevel.H);
            var qrCode = new QRCode(qrData);

            return qrCode.GetGraphic(pixelsPerModule);
        }

        public string GetFullUrl(string webCultureName = null)
        {
            string s = "";
            var c = BllFactory.Current.ConfigurationBll.Get();

            if (c.ProjectDomain.StartsWith("http", StringComparison.OrdinalIgnoreCase) == false)
            {
                s = String.Concat(c.WebOptions.UseSsl ? "https://" : "http://", c.ProjectDomain);
            }
            else
            {
                s = c.ProjectDomain;
            }

            s = s.ToLower();

            if (s.EndsWith("/") == false)
            {
                s = String.Concat(s, "/");
            }

            if (String.IsNullOrEmpty(webCultureName) == false)
            {
                s = String.Concat(s, webCultureName);
            }

            if (s.EndsWith("/") == false)
            {
                s = String.Concat(s, "/");
            }

            return s;
        }
    }
}
