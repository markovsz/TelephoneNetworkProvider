using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic;
using Entities.RequestFeatures;
using Entities.DataTransferObjects;
using TelephoneNetworkProvider.ActionFilters;

namespace TelephoneNetworkProvider.Controllers
{
    [Route("/operator-profile")]
    [Authorize(Roles = "Operator")]
    public class OperatorController : Controller
    {
        private IOperatorLogic _operatorLogic;

        public OperatorController(IOperatorLogic operatorLogic)
        {
            _operatorLogic = operatorLogic;
        }

        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [HttpGet("/operator-profile/calls/{id}")]
        public IActionResult GetCall(int id)
        {
            var call = _operatorLogic.GetCall(id);
            return Ok(call);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls")]
        public IActionResult GetCalls(CallParameters parameters)
        {
            var calls = _operatorLogic.GetCalls(parameters);
            return Ok(calls);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/operator-profile/customers/{customerId}/calls")]
        public IActionResult GetCustomerCalls(int customerId, CallParameters parameters)
        {
            var calls = _operatorLogic.GetCustomerCalls(customerId, parameters);//TODO
            return Ok(calls);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/operator-profile/customers/{customerId}")]
        public IActionResult GetCustomerInfo(int customerId)
        {
            var customer = _operatorLogic.GetCustomerInfo(customerId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/customers")]
        public IActionResult GetCustomers(CustomerParameters parameters)
        {
            var customers = _operatorLogic.GetCustomers(parameters);
            return Ok(customers);
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls/create")]
        public IActionResult CreateCall(CallForCreateInOperatorDto callDto)
        {
            _operatorLogic.CreateCall(callDto);
            return NoContent();
        }

        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [HttpGet("/operator-profile/calls/delete")]
        public IActionResult DeleteCall(int id)
        {
            _operatorLogic.DeleteCall(id);
            return NoContent();
        }
    }
}
