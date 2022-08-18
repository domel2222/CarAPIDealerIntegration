using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Authorization
{
    public class MultiDealerRequiment : IAuthorizationRequirement
    {
        public MultiDealerRequiment(int minDealer)
        {
            MinDealer = minDealer;
        }

        public int MinDealer { get; }
    }
}
