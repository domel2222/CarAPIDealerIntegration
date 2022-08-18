using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.DTOS
{
    public class CarCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string NameMark { get; set; }
        public string Model { get; set; }

        public decimal Price { get; set; }
        public int DealerId { get; set; }
    }
}
