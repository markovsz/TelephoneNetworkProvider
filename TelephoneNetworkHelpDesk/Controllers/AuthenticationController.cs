using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Repository;
using TelephoneNetworkProvider.ActionFilters;
using BussinessLogic;
using Microsoft.AspNetCore.Authorization;
using Logger;

namespace TelephoneNetworkProvider.Controllers
{
    [Route("login")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private IAuthenticationLogic _authenticationLogic;
        private ILoggerManager _logger;

        public AuthenticationController(IAuthenticationLogic authenticationLogic, ILoggerManager logger)
        {
            _authenticationLogic = authenticationLogic;
            _logger = logger;
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync(UserForAuthenticationDto user)
        {
            string role;
            try
            {
                role = await _authenticationLogic.ValidateUser(user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }

            var jwtToken = await _authenticationLogic.CreateToken();
            return Ok(jwtToken);
        }
    }
}
