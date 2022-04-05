﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;

namespace BussinessLogic
{
    public interface IAuthenticationLogic
    {
        Task<bool> ValidateUser(UserForAuthenticationDto user);
    }
}
