using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Repository;
using Repository.GuestRepository;
using BussinessLogic.Exceptions;

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

        public async Task<CustomerForReadInGuestDto> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _guestManager.Customers.GetCustomerAsync(customerId);
            if (customer is null)
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var customerDto = _mapper.Map<CustomerForReadInGuestDto>(customer);
            return customerDto;
        }

        public async Task<IEnumerable<CustomerForReadInGuestDto>> GetCustomersInfoAsync(CustomerParameters parameters)
        {
            var customers = await _guestManager.Customers.GetCustomersAsync(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInGuestDto>>(customers);
            return customersDto;
        }
    }
}