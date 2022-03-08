﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public interface ICallRepositoryForCustomer
    {
        IQueryable<Call> GetCalls(Guid customerId, Expression<Func<Customer, bool>> expression, CallParameters parameters);
    }
}
