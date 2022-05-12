using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Exceptions
{
    public class CustomerBlockedException : Exception
    {
        public CustomerBlockedException()
        {
        }

        public CustomerBlockedException(string message)
            : base(message)
        {
        }
    }
}
