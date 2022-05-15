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

        public async Task CreateCallAsync(CallForCreateInOperatorDto callDto)
        {
            var call = _mapper.Map<Call>(callDto);
            await _operatorManager.Calls.CreateCallAsync(call);
        }

        public async Task DeleteCallAsync(int id)
        {
            await _operatorManager.Calls.DeleteCallByIdAsync(id);
        }

        public async Task<CallForReadInOperatorDto> GetCallAsync(int id)
        {
            var calls = await _operatorManager.Calls.GetCallInfoAsync(id, false);
            var callsDto = _mapper.Map<CallForReadInOperatorDto>(calls);
            return callsDto;
        }

        public async Task<IEnumerable<CallForReadInOperatorDto>> GetCallsAsync(CallParameters parameters)
        {
            var calls = await _operatorManager.Calls.GetCallsAsync(parameters, false);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInOperatorDto>>(calls);
            return callsDto;
        }

        public async Task<IEnumerable<CallForReadInOperatorDto>> GetCustomerCallsAsync(int customerId, CallParameters parameters)
        {
            var calls = await _operatorManager.Calls.GetCustomerCallsAsync(customerId, parameters);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInOperatorDto>>(calls);
            return callsDto;
        }

        public async Task<CustomerForReadInOperatorDto> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _operatorManager.Customers.GetCustomerInfoAsync(customerId);
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
