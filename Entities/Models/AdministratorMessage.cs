using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class AdministratorMessage
    {
        public uint Id { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }
    }
}
