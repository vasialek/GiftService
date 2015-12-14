using AutoMapper;
using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{
    public static class AutoMapperConfigDal
    {
        public static void RegisterMapping()
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.SourceMemberNamingConvention = new PascalCaseNamingConvention();
            //    cfg.DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();
            //    //cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            //    //cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            //});

            Mapper.CreateMap<ProductBdo, product>()
                .ForMember(dest => dest.phone_reservation,
                            x => x.MapFrom(src => src.PhoneForReservation))
                .ReverseMap();

            Mapper.CreateMap<transaction, TransactionBdo>()
                .ForMember(dest => dest.PayerName,
                    orig => orig.MapFrom(x => x.p_name))
                .ForMember(dest => dest.PayerLastName,
                    orig => orig.MapFrom(x => x.p_lastname))
                .ForMember(dest => dest.PayerEmail,
                    orig => orig.MapFrom(x => x.p_email))
                .ForMember(dest => dest.RequestedAmount,
                    orig => orig.MapFrom(x => x.requested_amount))
                .ForMember(dest => dest.RequestedCurrencyCode,
                    orig => orig.MapFrom(x => x.requested_currency_code))
                .ForMember(dest => dest.ResponseFromPaymentSystem,
                    orig => orig.MapFrom(x => x.response_from_payment));
        }

        public static void SetMappingTypeFromLowerToPascal()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            });

            RegisterMapping();

            //Mapper.CreateMap<ProductBdo, product>()
            //    .ForMember(dest => dest.phone_reservation,
            //                x => x.MapFrom(src => src.PhoneForReservation))
            //    .ReverseMap();

            //Mapper.CreateMap<transaction, TransactionBdo>();
        }

        public static void SetMappingTypeFromPascalToLower()
        {
            Mapper.Configuration.SourceMemberNamingConvention = new PascalCaseNamingConvention();
            Mapper.Configuration.DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();

            RegisterMapping();

            //Mapper.CreateMap<ProductBdo, product>()
            //    .ForMember(dest => dest.phone_reservation,
            //                x => x.MapFrom(src => src.PhoneForReservation))
            //    .ReverseMap();

            //Mapper.CreateMap<transaction, TransactionBdo>();
        }
    }
}
