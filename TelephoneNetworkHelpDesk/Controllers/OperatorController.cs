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
        public async Task<IActionResult> GetCall(int id)
        {
            var call = await _operatorLogic.GetCallAsync(id);
            return Ok(call);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls")]
        public async Task<IActionResult> GetCallsAsync(CallParameters parameters)
        {
            var calls = await _operatorLogic.GetCallsAsync(parameters);
            return Ok(calls);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/operator-profile/customers/{customerId}/calls")]
        public async Task<IActionResult> GetCustomerCallsAsync(int customerId, CallParameters parameters)
        {
            var calls = await _operatorLogic.GetCustomerCallsAsync(customerId, parameters);//TODO
            return Ok(calls);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/operator-profile/customers/{customerId}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            var customer = await _operatorLogic.GetCustomerInfoAsync(customerId);
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync(CustomerParameters parameters)
        {
            var customers = await _operatorLogic.GetCustomersAsync(parameters);
            return Ok(customers);
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls/create")]
        public async Task<IActionResult> CreateCallAsync(CallForCreateInOperatorDto callDto)
        {
            await _operatorLogic.CreateCallAsync(callDto);
            return NoContent();
        }

        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [HttpGet("/operator-profile/calls/delete")]
        public async Task<IActionResult> DeleteCallAsync(int id)
        {
            await _operatorLogic.DeleteCallAsync(id);
            return NoContent();
        }
    }
}
