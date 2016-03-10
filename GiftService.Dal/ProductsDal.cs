using AutoMapper;
using GiftService.Models;
using log4net;
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
        ProductBdo GetProductByPaySystemUid(string paySystemUid);
        ProductBdo SaveProductInformationFromPos(ProductBdo product, PosBdo pos);
        string GetUniqueOrderId(int posId);
    }

    public class ProductsDal : IProductsDal
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

        public ProductsDal()
        {
        }


        public ProductBdo GetProductByUid(string productUid)
        {
            Logger.InfoFormat("Searching product information by UID: `{0}`", productUid);
            ProductBdo product = null;
            product p = null;

            try
            {
                using (var db = new GiftServiceEntities())
                {
                    p = db.products.First(x => x.product_uid.Equals(productUid, StringComparison.OrdinalIgnoreCase));
                }

                if (p != null)
                {
                    product = new ProductBdo
                    {
                        Id = p.id,
                        ProductUid = p.product_uid,
                        PosUserUid = p.pos_user_uid,
                        PaySystemUid = p.pay_system_uid,

                        ProductName = p.product_name,
                        ProductDescription = p.product_description,
                        ProductPrice = p.product_price,
                        CurrencyCode = p.currency_code,

                        CustomerName = p.customer_name,
                        CustomerEmail = p.customer_email,
                        CustomerPhone = p.customer_phone,
                        Remarks = p.remarks,

                        ValidFrom = p.valid_from.HasValue ? p.valid_from.Value : DateTime.MinValue,
                        ValidTill = p.valid_till.HasValue ? p.valid_till.Value : DateTime.MinValue,

                        PosId = p.pos_id,
                        PosName = p.pos_name,
                        PosCity = p.pos_city,
                        PosAddress = p.pos_address,
                        PosUrl = p.pos_url,

                        EmailForReservation = p.email_reservation,
                        PhoneForReservation = p.phone_reservation
                    };
                }

                return product;
            }
            catch (Exception ex)
            {
                Logger.Error("Error searching product by UID: " + productUid, ex);
                throw;
            }
        }

        public ProductBdo SaveProductInformationFromPos(ProductBdo product, PosBdo pos)
        {
            var p = new product();
            try
            {
                //AutoMapperConfigDal.SetMappingTypeFromBdoToDao();
                //p = Mapper.Map<product>(product);

                p.pos_user_uid = product.PosUserUid;
                p.pay_system_uid = product.PaySystemUid;

                p.product_uid = product.ProductUid;
                p.product_name = product.ProductName;
                p.product_description = product.ProductDescription;
                p.product_price = product.ProductPrice;
                p.currency_code = product.CurrencyCode;

                p.customer_name = product.CustomerName;
                p.customer_phone = product.CustomerPhone;
                p.customer_email = product.CustomerEmail;
                p.remarks = product.Remarks;

                p.email_reservation = product.EmailForReservation;
                p.phone_reservation = product.PhoneForReservation;

                p.pos_id = product.PosId;
                p.pos_name = product.PosName;
                p.pos_address = product.PosAddress;
                p.pos_city = product.PosCity;

                p.valid_from = product.ValidFrom;
                p.valid_till = product.ValidTill;

                Logger.Info("Saving product:");
                Logger.DebugFormat("  pos_user_uid:          `{0}`", p.pos_user_uid);
                Logger.DebugFormat("  pay_system_uid:        `{0}`", p.pay_system_uid);

                Logger.DebugFormat("  product_uid:           `{0}`", p.product_uid);
                Logger.DebugFormat("  product_name:          `{0}`", p.product_name);
                Logger.DebugFormat("  product_description:   `{0}`", p.product_description);
                Logger.DebugFormat("  product_price:         `{0}`", p.product_price);
                Logger.DebugFormat("  currency_code:         `{0}`", p.currency_code);

                Logger.DebugFormat("  customer_name:         `{0}`", p.customer_name);
                Logger.DebugFormat("  customer_phone:        `{0}`", p.customer_phone);
                Logger.DebugFormat("  customer_email:        `{0}`", p.customer_email);
                Logger.DebugFormat("  remarks:               `{0}`", p.remarks);

                Logger.DebugFormat("  email_reservation:     `{0}`", p.email_reservation);
                Logger.DebugFormat("  phone_reservation:     `{0}`", p.phone_reservation);

                Logger.DebugFormat("  pos_id:                `{0}`", p.pos_id);
                Logger.DebugFormat("  pos_name:              `{0}`", p.pos_name);
                Logger.DebugFormat("  pos_city:              `{0}`", p.pos_city);
                Logger.DebugFormat("  pos_address:           `{0}`", p.pos_address);
                Logger.DebugFormat("  valid_from:            `{0}`", p.valid_from);
                Logger.DebugFormat("  valid_till:            `{0}`", p.valid_till);
                //Logger.DebugFormat("  PaymentSystem:        `{0}`", p.Pay);
                using (var db = new GiftServiceEntities())
                {
                    db.products.Add(p);
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbvex)
            {
                Logger.Error("Validation error saving product", dbvex);
                foreach (var e in dbvex.EntityValidationErrors)
                {
                    foreach (var sub in e.ValidationErrors)
                    {
                        Logger.ErrorFormat("  {0,-16}: {1}", sub.PropertyName, sub.ErrorMessage);
                    }
                }
                throw;
            }

            return product;
        }

        public ProductBdo GetProductByPaySystemUid(string paySystemUid)
        {
            ProductBdo product = null;
            product productDao = null;

            Logger.InfoFormat("Searching for product information by payment system UID: `{0}`", paySystemUid);
            using (var db = new GiftServiceEntities())
            {
                productDao = db.products.First(x => paySystemUid.Equals(x.pay_system_uid, StringComparison.OrdinalIgnoreCase));
            }

            if (productDao != null)
            {
                product = new ProductBdo
                {
                    Id = productDao.id,
                    ProductUid = productDao.product_uid,
                    PosUserUid = productDao.pos_user_uid,
                    PaySystemUid = productDao.pay_system_uid,

                    ProductName = productDao.product_name,
                    ProductDescription = productDao.product_description,
                    ProductPrice = productDao.product_price,
                    CurrencyCode = productDao.currency_code,

                    CustomerName = productDao.customer_name,
                    CustomerEmail = productDao.customer_email,
                    CustomerPhone = productDao.customer_phone,
                    Remarks = productDao.remarks,

                    ValidFrom = productDao.valid_from.HasValue ? productDao.valid_from.Value : DateTime.MinValue,
                    ValidTill = productDao.valid_till.HasValue ? productDao.valid_till.Value : DateTime.MinValue,

                    PosId = productDao.pos_id,
                    PosName = productDao.pos_name,
                    PosCity = productDao.pos_city,
                    PosAddress = productDao.pos_address,
                    PosUrl = productDao.pos_url,

                    EmailForReservation = productDao.email_reservation,
                    PhoneForReservation = productDao.phone_reservation
                };
            }

            return product;
        }

        public string GetUniqueOrderId(int posId)
        {
            Logger.Info("Generating unique order ID (in database)");
            string orderId = null;
            using (var db = new GiftServiceEntities())
            {
                var r = db.unique_orderid_get(posId);
                orderId = r.First();
            }

            return orderId;
        }
    }
}
