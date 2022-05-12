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

namespace TelephoneNetworkProvider.Controllers
{
    [Route("login")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private IAuthenticationLogic _authenticationLogic;

        public AuthenticationController(IAuthenticationLogic authenticationLogic)
        {
            _authenticationLogic = authenticationLogic;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("yeah");
        }

        [ServiceFilter(typeof(DtoValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> Authentication(UserForAuthenticationDto user)
        {
            (bool status, string role) = await _authenticationLogic.ValidateUser(user);
            if (!status)
            {
                return BadRequest();//NO! (I don't remember)
            }

            var jwtToken = await _authenticationLogic.CreateToken();
            return Ok(jwtToken);
        }
    }
}
