using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;

namespace TelephoneNetworkProvider
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationDto, Customer>();

            CreateMap<Customer, CustomerForReadInAdministratorDto>().ReverseMap();
            CreateMap<Customer, CustomerForReadInCustomerDto>();
            CreateMap<Customer, CustomerForReadInGuestDto>().ReverseMap();
            CreateMap<Customer, CustomerForReadInOperatorDto>();

            CreateMap<CustomerForCreateInAdministratorDto, Customer>()
                .ForMember(c => c.IsPhoneNumberHided, c => c.Ignore())
                .BeforeMap((cf, c) => c.IsPhoneNumberHided = (cf.IsPhoneNumberHidedStr.Equals("true") ? true : false));

            CreateMap<Customer, CustomerForUpdateInAdministratorDto>();
            CreateMap<Customer, CustomerForUpdateInCustomerDto>();


            CreateMap<Call, CallForReadInAdministratorDto>();
            CreateMap<Call, CallForReadInCustomerDto>();
            CreateMap<Call, CallForReadInOperatorDto>();

            CreateMap<Call, CallForCreateInOperatorDto>();



            CreateMap<AdministratorMessage, AdministratorMessageForReadInAdministratorDto>();
            //CreateMap<AdministratorMessage, AdministratorMessageForReadInCustomerDto>();
            //CreateMap<AdministratorMessage, AdministratorMessageForReadInGuestDto>();
            //CreateMap<AdministratorMessage, AdministratorMessageForReadInOperatorDto>();
        }
    }
}
