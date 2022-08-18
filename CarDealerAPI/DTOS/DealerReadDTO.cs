using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.DTOS
{
    public class DealerReadDTO
    {
        public int Id { get; set; }
        public string DealerName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool TestDrive { get; set;}

        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public List<CarReadDTO> Cars { get; set; }

    }
}
