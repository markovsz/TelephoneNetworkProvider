using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Repository.OperatorRepository;
using BussinessLogic;
using AutoMapper;
using Entities.Models;
using BussinessLogic.Exceptions;

namespace BussinessLogic
{
    public class OperatorLogic : IOperatorLogic
    {
        private IOperatorManager _operatorManager;
        private IMapper _mapper;

        public OperatorLogic(IOperatorManager operatorManager, IMapper mapper)
        {
            _mapper = mapper;
            _operatorManager = operatorManager;
        }

        private async Task<bool> CheckCustomer(int customerId)
        {
            return (await _operatorManager.Customers.GetCustomerInfoAsync(customerId)) is not null;
        }

        private async Task<bool> CheckCall(int callId)
        {
            return (await _operatorManager.Calls.GetCallInfoAsync(callId, false)) is not null;
        }

        public async Task CreateCallAsync(CallForCreateInOperatorDto callDto)
        {
            var call = _mapper.Map<Call>(callDto);

            if (!(await CheckCustomer(call.CallerId)))
                throw new CustomerDoesntExistException("Customer with such caller id doesn't exist");

            if (!(await CheckCustomer(call.CalledById)))
                throw new CustomerDoesntExistException("Customer with such called by id doesn't exist");

            await _operatorManager.Calls.CreateCallAsync(call);
        }

        public async Task DeleteCallAsync(int callId)
        {
            if (!(await CheckCall(callId)))
                throw new CallDoesntExistException("Call with this id doesn't exist");

            await _operatorManager.Calls.DeleteCallByIdAsync(callId);
        }

        public async Task<CallForReadInOperatorDto> GetCallAsync(int callId)
        {
            var call = await _operatorManager.Calls.GetCallInfoAsync(callId, false);
            if(call is null)
                throw new CallDoesntExistException("Call with this id doesn't exist");
            var callDto = _mapper.Map<CallForReadInOperatorDto>(call);
            return callDto;
        }

        public async Task<IEnumerable<CallForReadInOperatorDto>> GetCallsAsync(CallParameters parameters)
        {
            var calls = await _operatorManager.Calls.GetCallsAsync(parameters, false);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInOperatorDto>>(calls);
            return callsDto;
        }

        public async Task<IEnumerable<CallForReadInOperatorDto>> GetCustomerCallsAsync(int customerId, CallParameters parameters)
        {
            if (!(await CheckCustomer(customerId)))
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var calls = await _operatorManager.Calls.GetCustomerCallsAsync(customerId, parameters);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInOperatorDto>>(calls);
            return callsDto;
        }

        public async Task<CustomerForReadInOperatorDto> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _operatorManager.Customers.GetCustomerInfoAsync(customerId);
            if(customer is null)
                throw new CustomerDoesntExistException("Customer with this id doesn't exist");

            var customerDto = _mapper.Map<CustomerForReadInOperatorDto>(customer);
            return customerDto;
        }

        public async Task<IEnumerable<CustomerForReadInOperatorDto>> GetCustomersAsync(CustomerParameters parameters)
        {
            var customers = await _operatorManager.Customers.GetCustomersAsync(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInOperatorDto>>(customers);
            return customersDto;
        }
    }
}
