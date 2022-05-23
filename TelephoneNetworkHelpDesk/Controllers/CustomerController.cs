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

namespace TelephoneNetworkProvider.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("/customer-profile")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerLogic _customerLogic;
        private readonly int _customerId;

        public CustomerController(ICustomerLogic customerLogic)
        {
            _customerLogic = customerLogic;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = identity.Claims
                .Where(c => c.Type.Equals(ClaimTypes.Name))
                .FirstOrDefault();
            _customerId = Int32.Parse(claim.Value);
        }

        [HttpGet("/customer-profile/info")]
        public async Task<IActionResult> GetInfoAsync()
        {
            CustomerForReadInCustomerDto customer;
            try
            {
                customer = await _customerLogic.GetCustomerInfoAsync(_customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(customer);
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpPut("/customer-profile/update")]
        public IActionResult UpdateCustomerInfo([FromBody] CustomerForUpdateInCustomerDto customerDto)
        {
            try
            {
                _customerLogic.UpdateCustomerInfo(_customerId, customerDto);
            }
            catch (CustomerDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [HttpPost("/customer-profile/replenish-balance")]
        public async Task<IActionResult> ReplenishTheBalanceAsync([FromBody] Decimal currency)
        {
            try
            {
                await _customerLogic.ReplenishTheBalanceAsync(_customerId, currency);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [ServiceFilter(typeof(CustomerExistenceFilterAttribute))]
        [HttpGet("/customer-profile/customers/{customerId}")]
        public async Task<IActionResult> GetCustomerInfoAsync(int customerId)
        {
            CustomerForReadInCustomerDto customer;
            try
            {
                customer = await _customerLogic.GetCustomerInfoAsync(customerId);
            }
            catch (CustomerDoesntExistException ex)
            {
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
                customers = await _customerLogic.GetCustomersAsync(parameters);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(customers);
        }
    }
}
