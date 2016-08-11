using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftService.Models;

namespace GiftService.Dal.Fakes
{
    public class FakeProductsDal : IProductsDal
    {
        private static List<ProductBdo> _products = null;

        public FakeProductsDal()
        {
            if (_products == null)
            {
                _products = new List<ProductBdo>();

                _products.Add(new ProductBdo
                {
                    Id = 1001,
                    PaySystemUid = "2b90a96612704938a0f73107d87d4fed",
                    PosUserUid = "2b90a96612704938a0f73107d87d4fed",
                    ProductUid = "2b90a96612704938a0f73107d87d4fed",

                    PosId = 1005,
                    PosAddress = "Kalvariju 2",
                    PosCity = "Vilnius",
                    PosName = "RitosMasazai.lt",
                    PosUrl = "http://www.ritosmasazai.lt",

                    ProductName = "Lengvas masazas",
                    ProductDescription = "Super lengvas atpalaidojantis masazas",
                    ProductDuration = "10 min",
                    ProductPrice = 10.50m,
                    CurrencyCode = "EUR",

                    CustomerEmail = "av@fscc.lt",
                    CustomerName = "Aleksej V.",
                    CustomerPhone = "+370 600 12345",

                    PhoneForReservation = "8 5 12345",
                    EmailForReservation = "info@ritosmasazai.lt",

                    ValidFrom = DateTime.Now.AddDays(-3),
                    ValidTill = DateTime.Now.AddMonths(3)

                });

                _products.Add(new ProductBdo
                {
                    Id = 1002,
                    PaySystemUid = "5d26299e25b444d3a7dc5b9f3aa0a8ef",
                    PosUserUid = "5d26299e25b444d3a7dc5b9f3aa0a8ef",
                    ProductUid = "5d26299e25b444d3a7dc5b9f3aa0a8ef",

                    PosId = 1005,
                    PosAddress = "Kalvariju 2",
                    PosCity = "Vilnius",
                    PosName = "RitosMasazai.lt",
                    PosUrl = "http://www.ritosmasazai.lt",

                    ProductName = "Lengvas masazas",
                    ProductDescription = "Super lengvas atpalaidojantis masazas",
                    ProductDuration = "10 min",
                    ProductPrice = 10.50m,
                    CurrencyCode = "EUR",

                    CustomerEmail = "av@fscc.lt",
                    CustomerName = "Aleksej V.",
                    CustomerPhone = "+370 600 12345",

                    PhoneForReservation = "8 5 12345",
                    EmailForReservation = "info@ritosmasazai.lt",

                    ValidFrom = DateTime.Now.AddDays(-3),
                    ValidTill = DateTime.Now.AddMonths(3)

                });
            }
        }

        public ProductBdo GetProductByPaySystemUid(string paySystemUid)
        {
            return _products.First(x => x.PaySystemUid == paySystemUid);
        }

        public ProductBdo GetProductByUid(string productUid)
        {
            return _products.First(x => x.ProductUid == productUid);
        }

        public string GetUniqueOrderId(int posId)
        {
            throw new NotImplementedException();
        }

        public void MakeProductGift(ProductBdo product, string friendEmail, string text)
        {
            throw new NotImplementedException();
        }

        public ProductBdo SaveProductInformationFromPos(ProductBdo product, PosBdo pos)
        {
            throw new NotImplementedException();
        }
    }
}
