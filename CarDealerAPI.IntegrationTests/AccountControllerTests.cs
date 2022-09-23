using CarDealerAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using CarDealerAPI.DTOS;
using CarDealerAPI.IntegrationTests.Helpers;
using CarDealerAPI.Services;

namespace CarDealerAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _httpClient;
        private string _apiAccountUrl = "api/account/";
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Program> webApplicationFactory)
        //public AccountControllerTests()
        {
            //_httpClient = new WebApplicationFactory<Program>() - with internals visible To
            _httpClient = webApplicationFactory
                        .WithWebHostBuilder(builder =>
                        {
                            builder.ConfigureServices(services =>
                            {
                                var dbContextOption = services
                                        .SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<DealerDbContext>));

                                services.Remove(dbContextOption);

                                var dbContextOption22 = services
                                        .SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<DealerDbContext>));

                                services.AddSingleton<IAccountService>(_accountServiceMock.Object);

                                services.AddDbContext<DealerDbContext>(options => options.UseInMemoryDatabase("DealerDb"));
                            });
                        
                        })
                        .CreateClient();
        }

        [Fact]
        public async Task CreateUserAccount_ForValidModel_ReturnsOk()
        {
            var userAccount = new UserCreateDTO()
            {
                Email = "userTest@gmail.com",
                Password = "password123",
                ConfirmPassword = "password123",
            };

            var httpContent = userAccount.ToJsonHttpContent();

            var response = await _httpClient.PostAsync(_apiAccountUrl + "register", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateUserAccount_InvalidModel_ReturnsBadRequest()
        {
            var userAccount = new UserCreateDTO()
            {
                Password = "password12",
                ConfirmPassword = "password123",
            };

            var httpContent = userAccount.ToJsonHttpContent();

            var response = await _httpClient.PostAsync(_apiAccountUrl + "register", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForCreatedUserAccount_ReturnsOk()
        {
            _accountServiceMock
                .Setup(x => x.GenerateToken(It.IsAny<UserLoginDTO>()))
                .Returns("JWT");

            var userLoginAccount = new UserLoginDTO()
            {
                Email = "userTest2@gmail.com",
                Password = "password123"
            };

            var httpContent = userLoginAccount.ToJsonHttpContent();

            var response = await _httpClient.PostAsync(_apiAccountUrl + "login", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
