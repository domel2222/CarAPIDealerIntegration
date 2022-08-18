using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.DTOS
{
    public class DealerUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string DealerName { get; set; }
        public string Description { get; set; }
        public bool TestDrive { get; set; }
    }
}
