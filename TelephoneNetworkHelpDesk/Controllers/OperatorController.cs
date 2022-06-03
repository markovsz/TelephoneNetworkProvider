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
using Logger;

namespace TelephoneNetworkProvider.Controllers
{
    [Authorize(Roles = "Operator")]
    [Route("/operator-profile")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private IOperatorLogic _operatorLogic;
        private ILoggerManager _logger;

        public OperatorController(IOperatorLogic operatorLogic, ILoggerManager logger)
        {
            _operatorLogic = operatorLogic;
            _logger = logger;
        }


        [HttpGet("/operator-profile/calls/call/{id:int}")]
        public async Task<IActionResult> GetCall(int id)
        {
            CallForReadInOperatorDto call;
            try
            {
                call = await _operatorLogic.GetCallInfoAsync(id);
            }
            catch (CallDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return Ok(call);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls")]
        public async Task<IActionResult> GetCallsAsync([FromQuery] CallParameters parameters)
        {
            IEnumerable<CallForReadInOperatorDto> calls;
            try
            {
                calls = await _operatorLogic.GetCallsInfoAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(calls);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/customers/customer/{customerId:int}/calls")]
        public async Task<IActionResult> GetCustomerCallsAsync(int customerId, [FromQuery] CallParameters parameters)
        {
            IEnumerable<CallForReadInOperatorDto> calls;
            try
            {
                calls = await _operatorLogic.GetCustomerCallsInfoAsync(customerId, parameters);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(calls);
        }


        [HttpGet("/operator-profile/customers/customer/{customerId:int}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            CustomerForReadInOperatorDto customer;
            try
            {
                customer = await _operatorLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/operator-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerParameters parameters)
        {
            IEnumerable<CustomerForReadInOperatorDto> customers;
            try
            {
                customers = await _operatorLogic.GetCustomersInfoAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpGet("/operator-profile/calls/create")]
        public async Task<IActionResult> CreateCallAsync([FromBody] CallForCreateInOperatorDto callDto)
        {
            try
            {
                await _operatorLogic.CreateCallAsync(callDto);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return NoContent();
        }


        [HttpGet("/operator-profile/calls/call/{callId:int}/delete")]
        public async Task<IActionResult> DeleteCallAsync(int callId)
        {
            try
            {
                await _operatorLogic.DeleteCallAsync(callId);
            }
            catch (CallDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
