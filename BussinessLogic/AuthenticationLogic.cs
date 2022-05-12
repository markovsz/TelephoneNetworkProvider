using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic;
using Entities.DataTransferObjects;
using Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Entities.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading;

namespace BussinessLogic
{
    public class AuthenticationLogic : IAuthenticationLogic
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleStore<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private User _user;

        public AuthenticationLogic(UserManager<User> userManager, IRoleStore<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<(bool, string)> ValidateUser(UserForAuthenticationDto user)
        {
            //CancellationToken token = new CancellationToken();
            //var result = await _roleManager.CreateAsync(new IdentityRole("Administrator"), token);
            //result = await _roleManager.CreateAsync(new IdentityRole("Customer"), token);
            //result = await _roleManager.CreateAsync(new IdentityRole("Operator"), token);

            //_userManager.CreateAsync(new User() { });


            //var userP = new User("iiii");
            //var result1 = await _userManager.CreateAsync(userP, "eeee2eeee2eeee");
            //var userP = await _userManager.FindByNameAsync("iiii");
            //var result2 = await _userManager.AddToRoleAsync(userP, "Administrator");


            _user = await _userManager.FindByNameAsync(user.Login);
            bool isValid = _user != null && await _userManager.CheckPasswordAsync(_user, user.Password);
            string role = null;
            if (isValid)
            {
                var roles = await _userManager.GetRolesAsync(_user);
                role = roles.FirstOrDefault();
            }
            return (isValid, role);
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.Id)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials
        signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires:
            DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
