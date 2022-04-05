using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class UserForAuthenticationDto
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
