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

        public void CreateCall(CallForCreateInOperatorDto callDto)
        {
            var call = _mapper.Map<Call>(callDto);
            _operatorManager.Calls.CreateCall(call);
        }

        public void DeleteCall(int id)
        {
            _operatorManager.Calls.DeleteCallById(id);
        }

        public CallForReadInOperatorDto GetCall(int id)
        {
            var calls = _operatorManager.Calls.GetCallInfo(id, false);
            var callsDto = _mapper.Map<CallForReadInOperatorDto>(calls);
            return callsDto;
        }

        public IEnumerable<CallForReadInOperatorDto> GetCalls(CallParameters parameters)
        {
            var calls = _operatorManager.Calls.GetCalls(parameters, false);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInOperatorDto>>(calls);
            return callsDto;
        }

        public IEnumerable<CallForReadInOperatorDto> GetCustomerCalls(int customerId, CallParameters parameters)
        {
            var calls = _operatorManager.Calls.GetCustomerCalls(customerId, parameters);
            var callsDto = _mapper.Map<IEnumerable<CallForReadInOperatorDto>>(calls);
            return callsDto;
        }

        public CustomerForReadInOperatorDto GetCustomerInfo(int customerId)
        {
            var customer = _operatorManager.Customers.GetCustomerInfo(customerId);
            var customerDto = _mapper.Map<CustomerForReadInOperatorDto>(customer);
            return customerDto;
        }

        public IEnumerable<CustomerForReadInOperatorDto> GetCustomers(CustomerParameters parameters)
        {
            var customers = _operatorManager.Customers.GetCustomers(parameters);
            var customersDto = _mapper.Map<IEnumerable<CustomerForReadInOperatorDto>>(customers);
            return customersDto;
        }
    }
}
