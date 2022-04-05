using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic;
using Entities.DataTransferObjects;
using Repository;
using AutoMapper;

namespace BussinessLogic
{
    public class AuthenticationLogic : IAuthenticationLogic
    {
        private IAuthenticationManager _authenticationManager;
        private IMapper _mapper;/*!*/

        public AuthenticationLogic(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto user)
        {
            return await _authenticationManager.ValidateUser(user);
        }
    }
}
