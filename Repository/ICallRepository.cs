using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repository
{
    public interface ICallRepository
    {
        IQueryable<Call> FindCallsByCondition(Expression<Func<IQueryable<Call>, bool>> expression);

    }
}
