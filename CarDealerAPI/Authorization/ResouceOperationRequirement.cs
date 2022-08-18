using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Authorization
{
    public class ResouceOperationRequirement : IAuthorizationRequirement
    {
        public  ResouceOperation resouceOperation { get; }

        public ResouceOperationRequirement(ResouceOperation resouceOperation)
        {
            this.resouceOperation = resouceOperation;
        }
    }
}
