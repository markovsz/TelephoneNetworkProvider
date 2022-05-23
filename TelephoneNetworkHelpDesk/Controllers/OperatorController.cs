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
using BussinessLogic.Exceptions;

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
            CallForReadInOperatorDto call;
            try
            {
                call = await _operatorLogic.GetCallAsync(id);
            }
            catch (CallDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(call);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls")]
        public async Task<IActionResult> GetCallsAsync(CallParameters parameters)
        {
            IEnumerable<CallForReadInOperatorDto> calls;
            try
            {
                calls = await _operatorLogic.GetCallsAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(calls);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/operator-profile/customers/{customerId}/calls")]
        public async Task<IActionResult> GetCustomerCallsAsync(int customerId, CallParameters parameters)
        {
            IEnumerable<CallForReadInOperatorDto> calls;
            try
            {
                calls = await _operatorLogic.GetCustomerCallsAsync(customerId, parameters);//TODO
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(calls);
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/operator-profile/customers/{customerId}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            CustomerForReadInOperatorDto customer;
            try
            {
                customer = await _operatorLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }

        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync(CustomerParameters parameters)
        {
            IEnumerable<CustomerForReadInOperatorDto> customers;
            try
            {
                customers = await _operatorLogic.GetCustomersAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls/create")]
        public async Task<IActionResult> CreateCallAsync(CallForCreateInOperatorDto callDto)
        {
            try
            {
                await _operatorLogic.CreateCallAsync(callDto);
            }
            catch (CustomerDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [ServiceFilter(typeof(CallExistenceFilterAttribute))]
        [HttpGet("/operator-profile/calls/delete")]
        public async Task<IActionResult> DeleteCallAsync(int id)
        {
            try
            {
                await _operatorLogic.DeleteCallAsync(id);
            }
            catch (CallDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
