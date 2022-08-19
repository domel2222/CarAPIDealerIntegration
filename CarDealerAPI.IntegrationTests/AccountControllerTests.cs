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
using CarDealerAPI.DTOS;
using CarDealerAPI.IntegrationTests.Helpers;

namespace CarDealerAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _httpClient;
        private string _apiAccountUrl = "api/account/";

        public AccountControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.
                        WithWebHostBuilder(builder =>
                        {
                            builder.ConfigureServices(services =>
                            {
                                var dbContextOption = services
                                        .SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<DealerDbContext>));

                                services.Remove(dbContextOption);

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
