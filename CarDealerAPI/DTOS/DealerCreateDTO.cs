using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.DTOS
{
    public class DealerCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string DealerName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool TestDrive { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        public string Country { get; set; }

    }
}
