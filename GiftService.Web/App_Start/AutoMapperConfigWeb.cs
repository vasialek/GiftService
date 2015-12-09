using AutoMapper;
using GiftService.Bll;
using GiftService.Models;
using GiftService.Models.JsonModels;
using GiftService.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftService.Web.App_Start
{
    public static class AutoMapperConfigWeb
    {
        public static void RegisterMapping()
        {
            Mapper.CreateMap<ProductBdo, PaymentRequestValidationResponse>()
                .ReverseMap();

            Mapper.CreateMap<PaymentRequestValidationResponse, ProductCheckoutModel>();
            Mapper.CreateMap<PaymentRequestValidationResponse, ProductCheckoutModel>()
                .ForMember(dest => dest.ProductValidTill,
                            x => x.MapFrom(src => BllFactory.Current.HelperBll.ConvertFromUnixTimestamp(src.ProductValidTillTm)));
        }
    }
}