using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.DataTransferObjects;
using BussinessLogic;
using System.Security.Claims;
using Entities.RequestFeatures;
using TelephoneNetworkProvider.ActionFilters;
using System.Threading.Tasks;
using BussinessLogic.Exceptions;
using System.Collections.Generic;
using Logger;

namespace TelephoneNetworkProvider.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("/customer-profile")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerLogic _customerLogic;
        private ILoggerManager _logger;

        public CustomerController(ICustomerLogic customerLogic, ILoggerManager logger)
        {
            _customerLogic = customerLogic;
            _logger = logger;
        }

        private string GetUserIdFromRequest()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = identity.Claims
                .Where(c => c.Type.Equals(ClaimTypes.Name))
                .FirstOrDefault();
            if (claim is null)
                throw new InvalidOperationException("user claim wasn't found");
            return claim.Value;
        }

        private async Task<int> GetCustomerIdFromRequest()
        {
            int customerId;
            string userId = GetUserIdFromRequest();
            try
            {
                customerId = await _customerLogic.GetCustomerIdAsync(userId);
            }
            catch (CustomerDoesntExistException) {
                string exMessage = "invalid user claim";
                _logger.LogError(exMessage);
                throw new InvalidOperationException(exMessage);
            }

            return customerId;
        }

        [HttpGet("/customer-profile/info")]
        public async Task<IActionResult> GetInfoAsync()
        {
            CustomerForReadInCustomerDto customer;
            try
            {
                int customerId = await GetCustomerIdFromRequest();
                customer = await _customerLogic.GetCustomerInfoAsync(customerId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }


        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpPut("/customer-profile/update")]
        public async Task<IActionResult> UpdateCustomerInfo([FromBody] CustomerForUpdateInCustomerDto customerDto)
        {
            try
            {
                int customerId = await GetCustomerIdFromRequest();
                await _customerLogic.UpdateCustomerInfo(customerId, customerDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return NoContent();
        }


        [HttpPost("/customer-profile/replenish-balance")]
        public async Task<IActionResult> ReplenishTheBalanceAsync([FromBody] Decimal currency)
        {
            try
            {
                int customerId = await GetCustomerIdFromRequest();
                await _customerLogic.ReplenishTheBalanceAsync(customerId, currency);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return NoContent();
        }


        [HttpGet("/customer-profile/customers/customer/{customerId:int}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            CustomerForReadInCustomerDto customer;
            try
            {
                customer = await _customerLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }


        [ServiceFilter(typeof(ParametersValidationFilterAttribute))]
        [HttpGet("/customer-profile/customers")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerParameters parameters)
        {
            IEnumerable<CustomerForReadInCustomerDto> customers;
            try
            {
                customers = await _customerLogic.GetCustomersInfoAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }
    }
}
