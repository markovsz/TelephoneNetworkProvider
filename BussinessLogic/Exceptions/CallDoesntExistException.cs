using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Exceptions
{
    public class CallDoesntExistException : Exception
    {
        public CallDoesntExistException(string message)
            : base(message)
        {
        }
    }
}
