using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net.Http;
using CarDealerAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CarDealerAPI.DTOS;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization.Policy;
using CarDealerAPI.IntegrationTests.Helpers;

namespace CarDealerAPI.IntegrationTests
{
    public class DealerControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _httpClient;
        private string _apiDealerUrl = "api/Dealer/";

        public DealerControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.
                        WithWebHostBuilder(builder =>
                        {
                            builder.ConfigureServices(services =>
                            {
                                var dbContextOption = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<DealerDbContext>));

                                services.Remove(dbContextOption);

                                services.AddSingleton<IPolicyEvaluator, FakePolicyevaluator>();

                                services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                                services.AddDbContext<DealerDbContext>(options => options.UseInMemoryDatabase("DealerDb"));
                            });
                        })
                        .CreateClient();
        }

        [Fact]
        public async Task CreateDealer_WithValidModel_ReturnsCreatedStatus()
        {
            var model = new DealerCreateDTO()
            {
                DealerName = "TestDealerTesla",
                City = "Sigapour",
                Street = "Pingpong"
            };

            var httpContent = model.ToJsonHttpContent();
            //act
            var response = await _httpClient.PostAsync(_apiDealerUrl, httpContent);
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

        }

        [Fact]
        public async Task CreateDealer_WithinvalidModel_ReturnsBadRequest()
        {
            var model = new DealerCreateDTO()
            {
                ContactEmail = "TestDealr@gmail.com",
                Description = "How about",
                ContactNumber = "222 888 111"
            };

            var httpContent = model.ToJsonHttpContent();
            var response = await _httpClient.PostAsync(_apiDealerUrl, httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAll_WithQueryParameters_ReturnOkResult()
        {
            var url = "/api/Dealer?PageSize=5&PageNumber=2";
            var response = await _httpClient.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("PageSize=5&PageNumber=2")]
        [InlineData("PageSize=15&PageNumber=4")]
        [InlineData("PageSize=10&PageNumber=8")]
        public async Task GetAll_WithMultipleQueryParameters_ReturnOkResult(string queryParams)
        {
            var url = "/api/Dealer?" + queryParams;
            var response = await _httpClient.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("PageSize=51&PageNumber=45")]
        [InlineData("PageSize=135&PageNumber=4")]
        [InlineData("PageSize=110&PageNumber=8")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetAll_WithMultipleInvalidQueryParameters_ReturnBadRequest(string queryParams)
        {
            var url = "/api/Dealer?" + queryParams;
            var response = await _httpClient.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
