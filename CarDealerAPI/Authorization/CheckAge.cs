using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Authorization
{
    public class CheckAge : IAuthorizationRequirement
    {
        public int MinAge { get;  }
        public CheckAge(int minAge)
        {
            MinAge = minAge;
        }
    }
}
