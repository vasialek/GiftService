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

            Mapper.CreateMap<transaction, TransactionBdo>();
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
