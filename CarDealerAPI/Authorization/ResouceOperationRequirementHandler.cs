using CarDealerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Authorization
{
    public class ResouceOperationRequirementHandler : AuthorizationHandler<ResouceOperationRequirement, Dealer>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResouceOperationRequirement requirement, Dealer dealer)
        {
            if (requirement.resouceOperation == ResouceOperation.Read ||
                requirement.resouceOperation == ResouceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (dealer.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
