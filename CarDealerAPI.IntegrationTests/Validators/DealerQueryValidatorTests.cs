using CarDealerAPI.Extensions;
using CarDealerAPI.Extensions.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarDealerAPI.IntegrationTests.Validators
{
    public class DealerQueryValidatorTests
    {
        [Fact]
        public void Validate_ForValidModel_ReturnsSuccess()
        {
            var validator = new DealerQuerySearchValidator();
            var model = new DealerQuerySearch()
            {
                PageNumber = 1,
                PageSize = 10
                
            };
        }
    }
}
