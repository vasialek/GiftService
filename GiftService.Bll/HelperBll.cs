using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class HelperBll : IHelperBll
    {
        private static char[] HX = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

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
    }
}
