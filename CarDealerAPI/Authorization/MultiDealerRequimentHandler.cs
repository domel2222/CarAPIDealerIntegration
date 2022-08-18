using CarDealerAPI.Contexts;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Authorization
{
    public class MultiDealerRequimentHandler : AuthorizationHandler<MultiDealerRequiment>
    {
        private readonly DealerDbContext _dealerDbContext;

        public MultiDealerRequimentHandler(DealerDbContext dealerDbContext)
        {
            this._dealerDbContext = dealerDbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MultiDealerRequiment requirement)
        {
            var userId =   int.Parse(context.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var dealerCount = _dealerDbContext
                .Dealers
                .Count(d => d.CreatedById == userId);

            if (dealerCount >= requirement.MinDealer)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
