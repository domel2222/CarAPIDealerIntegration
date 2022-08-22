using CarDealerAPI.Extensions;
using CarDealerAPI.Extensions.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using CarDealerAPI.Models;

namespace CarDealerAPI.IntegrationTests.Validators
{
    public class DealerQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<DealerQuerySearch>()
            {
                new DealerQuerySearch()
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                new DealerQuerySearch()
                {
                    PageNumber = 5,
                    PageSize = 15,
                    SortBy = nameof(Dealer.DealerName)
                },
                new DealerQuerySearch()
                {
                    PageNumber = 13,
                    PageSize = 15,
                    SortBy = nameof(Dealer.Description)
                },
                new DealerQuerySearch()
                {
                    PageNumber = 2,
                    PageSize = 5
                }
            };

            return list.Select(x => new object[] { x });
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<DealerQuerySearch>()
            {
                new DealerQuerySearch()
                {
                    PageNumber = 4,
                    PageSize = 13
                },
                new DealerQuerySearch()
                {
                    PageNumber = 5,
                    PageSize = 15,
                    SortBy = nameof(Dealer.ContactNumber)
                },
                new DealerQuerySearch()
                {
                    PageNumber = 10,
                    PageSize = 15,
                    SortBy = nameof(Dealer.Cars)
                },
                new DealerQuerySearch()
                {
                    PageNumber = 7,
                    PageSize = 7
                }
            };

            return list.Select(x => new object[] { x });
        }

        [Fact]
        public void Validate_ForValidModel_ReturnsSuccess()
        {
            var model = new DealerQuerySearch()
            {
                PageNumber = 1,
                PageSize = 10
            };

            var validator = new DealerQuerySearchValidator();

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForValidModels_ReturnsSuccess(DealerQuerySearch model)
        {
            var validator = new DealerQuerySearchValidator();

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForInvalidModels_ReturnsFailure(DealerQuerySearch model)
        {
            var validator = new DealerQuerySearchValidator();

            var result = validator.TestValidate(model);

            result.ShouldHaveAnyValidationError();
        }
    }
}
