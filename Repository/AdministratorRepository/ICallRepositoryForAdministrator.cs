﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.AdministratorRepository
{
    public interface ICallRepositoryForAdministrator
    {
        Call GetCall(uint id);
        IEnumerable<Call> GetCalls(CallParameters parameters);
        IEnumerable<Call> GetCustomerCalls(uint customerId, CallParameters parameters);
    }
}