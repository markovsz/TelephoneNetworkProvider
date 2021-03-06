using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Repository.CustomerRepository;
using BussinessLogic.Exceptions;

namespace BussinessLogic
{
    public class CustomerLogic : ICustomerLogic
    {
        private ICustomerManager _customerManager;
        private IMapper _mapper;

        public CustomerLogic(ICustomerManager customerManager, IMapper mapper)
        {
            _customerManager = customerManager;
            _mapper = mapper;
        }

        private async Task<bool> CheckCustomer(int customerId)
        {
            return (await _customerManager.Customers.GetCustomerAsync(customerId, false)) is not null;
        }

        private async Task<Customer> GetCustomerAsync(int customerId, bool trackChanges)
        {
            var customer = await _customerManager.Customers.GetCustomerAsync(customerId, trackChanges);
            if (customer is null)
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");
            return customer;
        }

        public async Task UpdateCustomerInfo(int customerId, CustomerForUpdateInCustomerDto customerDto)
        {
            if (!(await CheckCustomer(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Id = customerId;
            _customerManager.Customers.UpdateCustomer(customer);
        }

        public async Task ReplenishTheBalanceAsync(int customerId, Decimal currency)
        {
            if (currency < 0.0m || currency > 50000.0m)
                throw new ArgumentOutOfRangeException("Currency should be between 0.0 and 50000.0");
            var customer = await GetCustomerAsync(customerId, true);
            customer.MoneyBalance += currency;
            await _customerManager.SaveAsync();
        }

        public async Task<IEnumerable<CallForReadInCustomerDto>> GetCallsInfoAsync(int customerId, CallParameters parameters)
        {
            if (!(await CheckCustomer(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var calls = await _customerManager.Calls.GetCallsAsync(customerId, parameters);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInCustomerDto>>(calls);
            return callsDto;
        }

        public async Task<CallForReadInCustomerDto> GetCallInfoAsync(int callId)
        {
            var call = await _customerManager.Calls.GetCallAsync(callId);
            if (call is null)
                throw new CallDoesntExistException("Call with this id doesn't exist");

            var callDto = _mapper.Map<CallForReadInCustomerDto>(call);
            return callDto;
        }

        public async Task<CustomerForReadInCustomerDto> GetCustomerInfoAsync(int customerId)
        {
            var customer = await GetCustomerAsync(customerId, false);
            var customerDto = _mapper.Map<CustomerForReadInCustomerDto>(customer);
            return customerDto;
        }

        public async Task<CustomerForReadInCustomerDto> GetCustomerInfoByUserIdAsync(string userId)
        {
            var customer = await _customerManager.Customers.GetCustomerByUserIdAsync(userId, false);
            var customerDto = _mapper.Map<CustomerForReadInCustomerDto>(customer);
            return customerDto;
        }

        public async Task<IEnumerable<CustomerForReadInCustomerDto>> GetCustomersInfoAsync(CustomerParameters parameters)
        {
            var customers = await _customerManager.Customers.GetCustomersAsync(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInCustomerDto>>(customers);
            return customersDto;
        }

        public async Task<int> GetCustomerIdAsync(string userId)
        {
            var customer = await _customerManager.Customers.GetCustomerByUserIdAsync(userId, false);
            if(customer is null)
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");
            return customer.Id;
        }
    }
}
