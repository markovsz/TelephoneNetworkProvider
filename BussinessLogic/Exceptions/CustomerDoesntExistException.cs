using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Exceptions
{
    public class CustomerDoesntExistException : Exception //404
    {
        public CustomerDoesntExistException(string message)
            : base(message)
        {
        }
    }
}
