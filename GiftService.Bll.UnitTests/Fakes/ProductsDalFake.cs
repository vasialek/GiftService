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

        public static IList<ProductBdo> GetProducts()
        {
            var products = new List<ProductBdo>();

            products.Add(new ProductBdo
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

            return products;
        }

    }
}
