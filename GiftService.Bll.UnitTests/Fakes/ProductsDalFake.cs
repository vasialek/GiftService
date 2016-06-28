using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll.UnitTests.Fakes
{
    public class ProductsDalFake
    {

        private static IList<ProductBdo> _products = null;

        public static IList<ProductBdo> GetProducts()
        {
            if (_products != null)
            {
                return _products;
            }

            _products = new List<ProductBdo>();

            _products.Add(new ProductBdo
            {
                ProductUid = Guid.NewGuid().ToString("N"),
                PaySystemUid = Guid.NewGuid().ToString("N"),
                PosUserUid = Guid.NewGuid().ToString("N"),

                PosId = 1006,
                PosName = "Knygynas \"Humanitas\"",
                PosCity = "Kaunas",
                PosAddress = "Butrimonių g. 9",
                PosUrl = "www.knygynai.lt",

                CustomerName = "Aleksej Vvvv",
                CustomerEmail = "prog.lamer@gmail.com",
                CustomerPhone = "+370 600 14789",
                Remarks = "Some remarks...",

                ProductName = "100 istorinių Vilniaus reliktų - Darius Pocevičius",
                ProductDescription = "Per pastaruosius 100 metų valdžia Vilniuje keitėsi net 14 kartų. Kiekviena trynė ankstesniųjų paliktus ženklus ir kūrė savuosius. Daug senienų sunaikino urbanizacija ir laikas. Vis dėlto mieste išliko daugybė istorinių reliktų, menančių LDK ir ATR epochas, Rusijos imperijos ir tarpukario laikotarpius. Ši knyga kviečia atrasti gyvąją miesto istoriją, kurią pasakoja fundacinės lentelės, statinių likučiai, užrašai, monogramos, herbai, paminklai, dekoro elementai. Jų atskleista senojo Vilniaus istorija skiriasi nuo perdėm romantizuotos, mitologizuotos ir lituanizuotos versijos, dažnai sutinkamos istorinėse apybraižose ir turistinėse brošiūrose. Istorinį detektyvą primenancios reliktų paieškos smarkiai išplečia turistinę miesto topografiją, paprastai apsiribojančia 10–15 lankytinų vietų, ir ragina pasidairyti toliau nuo centrinių gatvių. Tą padaryti padės leidinyje patalpinti 59 žemėlapiai, 72 dokumentų atvaizdai, 122 piešiniai, 195 brėžiniai, 272 fotografijos, 941 naudotos literatūros ir archyvinių dokumentų",
                //ProductDuration = "2-3 dienos",
                ProductPrice = 18.90m,
                CurrencyCode = "EUR",

                ValidFrom = DateTime.UtcNow,
                ValidTill = DateTime.UtcNow.AddMonths(3),

                PhoneForReservation = "+370 652 98422"
            });

            _products.Add(new ProductBdo
            {
                ProductUid = Guid.NewGuid().ToString("N"),
                PaySystemUid = Guid.NewGuid().ToString("N"),
                PosUserUid = Guid.NewGuid().ToString("N"),

                PosId = 1005,
                PosName = "BABOR GROŽIO CENTRAS",
                PosCity = "Wilno ųųųųų",
                PosAddress = "Juozapavičiaus g. 9A - 174",
                PosUrl = "www.url.com",

                CustomerName = "Aleksej Vvvv",
                CustomerEmail = "proglamer@gmail.com",
                CustomerPhone = "+370 600 14789",
                Remarks = "Some remarks...",

                ProductName = "Grožiodeivė & Undinėlė",
                ProductDescription = "Atstatantis viso kūno įvyniojimas su šveitimu išsausėjusiai kūno odai",
                ProductPrice = 12345.67m,
                CurrencyCode = "EUR",

                ValidFrom = DateTime.UtcNow,
                ValidTill = DateTime.UtcNow.AddMonths(3),

                PhoneForReservation = "+370 652 98422"
            });

            _products.Add(new ProductBdo
            {
                ProductUid = Guid.NewGuid().ToString("N"),
                PaySystemUid = Guid.NewGuid().ToString("N"),
                PosUserUid = Guid.NewGuid().ToString("N"),

                PosId = 1007,
                PosName = "UAB \"Melisanda\"",
                PosCity = "Wilno ųųųųų",
                PosAddress = "Kalvariju g. 119 / Apkasu g. 2",
                PosUrl = "www.melisanda.lt",

                CustomerName = "Aleksej Vvvv",
                CustomerEmail = "proglamer@gmail.com",
                CustomerPhone = "+370 600 14789",
                Remarks = "Some remarks...",

                ProductName = "Moteriškas kirpimas (plaukų plovimas, kirpimas, džiovinimas fenu)",
                ProductDescription = "",
                ProductDuration = "",
                ProductPrice = 12345.67m,
                CurrencyCode = "EUR",

                ValidFrom = DateTime.UtcNow,
                ValidTill = DateTime.UtcNow.AddMonths(3),

                PhoneForReservation = "+370 630 06009",

                TextForGift = "Sveikinu, Henrika su alkotesteriu :) Sveikinu, Henrika su alkotesteriu :) Sveikinu, Henrika su alkotesteriu :) Sveikinu, Henrika su alkotesteriu :) Sveikinu, Henrika su alkotesteriu :) Sveikinu, Henrika su alkotesteriu :) Sveikinu, Henrika"
            });

            return _products;
        }

    }
}
