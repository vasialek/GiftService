using GiftService.Dal;
using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{

    public interface IProductsBll
    {
        ProductInformationModel GetProductInformationByUid(string productUid);
    }

    public class ProductsBll : IProductsBll
    {
        private IProductsDal _productsDal = null;
        private IPosDal _posDal = null;

        public ProductsBll(IProductsDal productsDal, IPosDal posDal)
        {
            if (productsDal == null)
            {
                throw new ArgumentNullException("productsDal");
            }

            if (posDal == null)
            {
                throw new ArgumentNullException("posDal");
            }

            _productsDal = productsDal;
            _posDal = posDal;
        }

        public ProductInformationModel GetProductInformationByUid(string productUid)
        {
            ProductInformationModel model = new ProductInformationModel();

            model.Product = _productsDal.GetProductByUid(productUid);
            model.Pos = _posDal.GetById(model.Product.PosId);

            return model;
        }
    }
}
