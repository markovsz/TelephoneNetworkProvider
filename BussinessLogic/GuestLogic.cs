using System;
using System.Collections.Generic;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Repository;
using Repository.GuestRepository;

namespace BussinessLogic
{
    public class GuestLogic : IGuestLogic
    {
        private IGuestManager _guestManager;
        private IMapper _mapper;

        public GuestLogic(IGuestManager guestManager, IMapper mapper)
        {
            _guestManager = guestManager;
            _mapper = mapper;
        }

        public CustomerForReadInGuestDto GetCustomerInfo(int customerId)
        {
            var customers = _guestManager.Customers.GetCustomerInfo(customerId);
            var customersDto = _mapper.Map<CustomerForReadInGuestDto>(customers);
            return customersDto;
        }

        public IEnumerable<CustomerForReadInGuestDto> GetCustomers(CustomerParameters parameters)
        {
            var customers = _guestManager.Customers.GetCustomers(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInGuestDto>>(customers);
            return customersDto;
        }
    }
}