using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract class AdministratorMessageForRead
    {
        [Required]
        public uint CustomerId { get; set; }


        [Required]
        [MaxLength(20)]
        public string Status { get; set; }


        [MaxLength(20)]
        public string Text { get; set; }
    }
}
