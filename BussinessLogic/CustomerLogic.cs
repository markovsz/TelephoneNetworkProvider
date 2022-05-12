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

namespace BussinessLogic
{
    public class CustomerLogic : ICustomerLogic
    {
        private ICustomerManager _customerManager;
        //private ICustomerDataAcquisitionLogic<CustomerForReadInCustomerDto> _customerDataAcquisitionLogic;
        private IMapper _mapper;

        public CustomerLogic(ICustomerManager customerManager, IMapper mapper)
        {
            _customerManager = customerManager;
            _mapper = mapper;
        }

        public void UpdateCustomerInfo(int customerId, CustomerForUpdateInCustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Id = customerId;
            _customerManager.Customer.UpdateCustomer(customer);
        }

        public void ReplenishTheBalance(int customerId, Decimal currency)
        {
            if (currency < 0.0m || currency > 50000.0m)
                throw new ArgumentOutOfRangeException("Currency should be between 0.0 and 50000.0");
            var customer = _customerManager.Customer.GetCustomer(customerId, true);
            customer.MoneyBalance += currency;
        }

        public IEnumerable<CallForReadInCustomerDto> GetCalls(int customerId, CallParameters parameters)
        {
            var calls = _customerManager.Calls.GetCalls(customerId, parameters);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInCustomerDto>>(calls);
            return callsDto;
        }

        public CallForReadInCustomerDto GetCall(int id)
        {
            var call = _customerManager.Calls.GetCall(id);
            var callDto = _mapper.Map<CallForReadInCustomerDto>(call);
            return callDto;
        }

        public CustomerForReadInCustomerDto GetCustomerInfo(int customerId)
        {
            var customer = _customerManager.Customer.GetCustomer(customerId, false);
            var customerDto = _mapper.Map<CustomerForReadInCustomerDto>(customer);
            return customerDto;
        }

        public IEnumerable<CustomerForReadInCustomerDto> GetCustomers(CustomerParameters parameters)
        {
            var customers = _customerManager.Customer.GetCustomers(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInCustomerDto>>(customers);
            return customersDto;
        }
    }
}
