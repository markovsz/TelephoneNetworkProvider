using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Exceptions
{
    public class SendMessageException : Exception
    {
        public SendMessageException()
        {
        }

        public SendMessageException(string message)
            : base(message)
        {
        }
    }
}
