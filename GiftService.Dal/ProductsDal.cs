using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{
    public interface IProductsDal
    {
        ProductBdo GetProductByUid(string productUid);
    }

    public class ProductsDal : IProductsDal
    {
        List<ProductBdo> _products = new List<ProductBdo>();

        public ProductsDal()
        {
            ProductBdo p;

            p = new ProductBdo();
            p.Id = 100010;
            p.ProductUid = "PUID_000010000000000000000000000";
            p.PosId = 1005;
            p.CustomerName = "Aleksej Tak";
            p.ProductName = "Product name 1";
            p.ProductDescription = "Some text about product 1";
            p.ProductPrice = 100.01m;
            p.CurrencyCode = "EUR";

            p.PosName = "BABOR GROŽIO CENTRAS";
            p.PosUrl = "www.ritosmasazai.lt";
            p.PosCity = "Vilnius";
            p.PosAddress = "Juozapavičiaus g. 9A - 174";

            p.PhoneForReservation = "8 652 98422";

            p.ValidFrom = DateTime.Now.AddDays(-10);
            p.ValidTill = p.ValidFrom.AddYears(1);
            _products.Add(p);

            p = new ProductBdo();
            p.Id = 100011;
            p.ProductUid = "PUID_000020000000000000000000000";
            p.PosId = 1005;
            p.CustomerName = "Aleksej Tak";
            p.ProductName = "Product name 2";
            p.ProductDescription = "Some text about product 2";
            p.ProductPrice = 100.22m;
            p.CurrencyCode = "EUR";

            p.PosName = "BABOR GROŽIO CENTRAS";
            p.PosUrl = "www.ritosmasazai.lt";
            p.PosCity = "Vilnius";
            p.PosAddress = "Juozapavičiaus g. 9A - 174";

            p.PhoneForReservation = "8 652 98422";

            p.ValidFrom = DateTime.Now.AddDays(-17);
            p.ValidTill = p.ValidFrom.AddYears(1);
            _products.Add(p);
        }


        public ProductBdo GetProductByUid(string productUid)
        {
            return _products.First(x => x.ProductUid == productUid);
        }
    }
}
