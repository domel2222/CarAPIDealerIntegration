using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using CarDealerAPI.Extensions.Validators;
using CarDealerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentValidation.TestHelper;

namespace CarDealerAPI.IntegrationTests.Validators
{
    public class RegisterDtoValidatorTests
    {
        private readonly DealerDbContext _dbContext;
        public RegisterDtoValidatorTests()
        {
            //var config = new ConfigurationBuilder()
            //    .Build();
            var builder = new DbContextOptionsBuilder<DealerDbContext>();
            builder.UseInMemoryDatabase("TestDbDealer");
            _dbContext = new DealerDbContext(builder.Options);
            Seed();
        }

        [Fact]
        public void Validate_ForValidModel_ReturnsSuccess()
        {
            var model = new UserCreateDTO()
            {
                Email = "Mytest@gmail.com",
                Password = "mypass123",
                ConfirmPassword = "mypass123"
            };

            var validator = new RegisterDtoValidator(_dbContext);

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void ValidateMultipleValidModel_ReturnSuccess(UserCreateDTO model)
        {
            var validator = new RegisterDtoValidator(_dbContext);

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ForInvalidModel_ReturnsFaild()
        {
            var model = new UserCreateDTO()
            {
                Email = "testFirst@gmail.com",
                Password = "mytest321",
                ConfirmPassword = "mytest321"
            };

            var validator = new RegisterDtoValidator(_dbContext);

            var result = validator.TestValidate(model);

            result.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForMultipleInvalidModel_ReturnsFaild(UserCreateDTO model)
        {
            var validator = new RegisterDtoValidator(_dbContext);

            var result = validator.TestValidate(model);

            result.ShouldHaveAnyValidationError();
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<UserCreateDTO>()
            {
                new UserCreateDTO()
                {
                    Email = "testSecond@gmail.com",
                    Password = "mytest321",
                    ConfirmPassword = "mytest321"
                },
                new UserCreateDTO()
                {
                    Email = "",
                    Password = "ggggg451",
                    ConfirmPassword = "ggggg451"
                },
                new UserCreateDTO()
                {
                    Email = "mamyTest@gmail.com",
                    Password = "xx12",
                    ConfirmPassword = "xx12"
                }
            };

            return list.Select(x => new object[] { x });
        }

        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<UserCreateDTO>()
            {
                new UserCreateDTO()
                {
                    Email = "MySyperTest@op.pl",
                    Password = "superpass321",
                    ConfirmPassword = "superpass321"
                },
                new UserCreateDTO()
                {
                    Email = "SuperSayanTest@wp.pl",
                    Password = "goku6541",
                    ConfirmPassword = "goku6541"
                },
                new UserCreateDTO()
                {
                    Email = "tenTest@buziaczek.pl",
                    Password = "kissyou12",
                    ConfirmPassword = "kissyou12"
                },
            };

            return list.Select(x => new object[] { x });
        }

        private void Seed()
        {
            var userForTest = new List<User>()
            {
                new User ()
                {
                    Email = "testFirst@gmail.com"
                },

                new User()
                {
                    Email = "testSecond@gmail.com"
                }
            };

            _dbContext.Users.AddRange(userForTest);
            _dbContext.SaveChanges();
        }

    }
}
