using AutoMapper;
using GiftService.Dal;
using GiftService.Models;
using GiftService.Models.JsonModels;
using GiftService.Models.Products;
using log4net;
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
        ProductBdo GetProductByPaySystemUid(string paySystemUid);
        ProductBdo SaveProductInformationFromPos(string posUserUid, PaymentRequestValidationResponse posResponse, ProductCheckoutModel checkout);
    }

    public class ProductsBll : IProductsBll
    {
        private ILog _logger = null;
        private ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(GetType());
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }


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

        public ProductBdo SaveProductInformationFromPos(string posUserUid, PaymentRequestValidationResponse posResponse, ProductCheckoutModel checkout)
        {
            if (posResponse == null)
            {
                throw new ArgumentNullException("Response from POS is NULL");
            }

            ProductBdo product = new ProductBdo();
            PosBdo pos = new PosBdo();

            // TODO: use automapper
            Mapper.CreateMap<PaymentRequestValidationResponse, ProductBdo>()
                .ForMember(dest => dest.ProductPrice,
                            orig => orig.MapFrom(x => x.RequestedAmountMinor / 100m))
                .ForMember(dest => dest.ValidTill,
                            orig => orig.MapFrom(x => BllFactory.Current.HelperBll.ConvertFromUnixTimestamp(x.ProductValidTillTm)));

            product = Mapper.Map<ProductBdo>(posResponse);

            if (checkout.LocationId > 0)
            {
                var location = posResponse.Locations.FirstOrDefault(x => x.Id == checkout.LocationId);
                if (location == null)
                {
                    throw new ArgumentOutOfRangeException("LocationId", "Incorrect POS service location!");
                }

                product.PosName = location.Name;
                product.PosCity = location.City;
                product.PosAddress = location.Address;
            }



            product.CustomerName = checkout.CustomerName;
            product.CustomerEmail = checkout.CustomerEmail;
            product.CustomerPhone = checkout.CustomerPhone;
            product.Remarks = checkout.Remarks;

            product.PaymentSystem = checkout.PaymentSystem;
            product.ProductUid = Guid.NewGuid().ToString("N");
            product.PosUserUid = posUserUid;
            product.PaySystemUid = Guid.NewGuid().ToString("N");

            return _productsDal.SaveProductInformationFromPos(product, pos);
        }

        public ProductBdo GetProductByPaySystemUid(string paySystemUid)
        {
            if (String.IsNullOrEmpty(paySystemUid))
            {
                throw new ArgumentNullException("paySystemUid", "Payment system UID should be non-empty");
            }

            try
            {
                return _productsDal.GetProductByPaySystemUid(paySystemUid);
            }
            catch (Exception ex)
            {
                Logger.Error("Error getting information about bought product by payment system UID: " + paySystemUid, ex);
                throw;
            }
        }
    }
}
