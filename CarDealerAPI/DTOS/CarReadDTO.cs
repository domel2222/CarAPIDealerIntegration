using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.DTOS
{
    public class CarReadDTO
    {
        public int Id { get; set; }
        public string NameMark { get; set; }
        public string Model { get; set; }

        public decimal Price { get; set; }
    }
}
