using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.DTOS
{
    public class UserReadDTO
    {
        public string ColorEye { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string  RoleId { get; set; } 
    }
}
