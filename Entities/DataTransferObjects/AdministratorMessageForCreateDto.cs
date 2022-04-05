using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AdministratorMessageForCreateDto
    {
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
    }
}
