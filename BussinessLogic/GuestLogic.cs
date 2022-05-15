using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<CustomerForReadInGuestDto> GetCustomerInfoAsync(int customerId)
        {
            var customers = await _guestManager.Customers.GetCustomerInfoAsync(customerId);
            var customersDto = _mapper.Map<CustomerForReadInGuestDto>(customers);
            return customersDto;
        }

        public async Task<IEnumerable<CustomerForReadInGuestDto>> GetCustomersAsync(CustomerParameters parameters)
        {
            var customers = await _guestManager.Customers.GetCustomersAsync(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInGuestDto>>(customers);
            return customersDto;
        }
    }
}