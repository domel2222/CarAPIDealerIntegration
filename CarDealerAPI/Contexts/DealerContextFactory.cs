using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarDealerAPI.Contexts
{
    public class DealerContextFactory : IDesignTimeDbContextFactory<DealerDbContext>
    {
        public DealerDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return new DealerDbContext(new DbContextOptionsBuilder<DealerDbContext>().Options, config);
        }
    }
}
