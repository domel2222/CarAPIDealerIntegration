using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CarDealerAPI.IntegrationTests
{
    public class CarControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private string _uriStart = "api/dealer/";
        private string _uriEnd = "/car";
        private WebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;

        public CarControllerTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _factory = webApplicationFactory
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            //var dbContextOption = services.SingleOrDefault(ser => ser.ServiceType == typeof(DbContextOptions<DealerDbContext>));

                            //services.Remove(dbContextOption);

                            services.AddSingleton<IPolicyEvaluator, FakePolicyevaluator>();

                            services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                            //services.AddDbContext<DealerDbContext>(option => option.UseInMemoryDatabase("DealerDb"));
                        });
                    });
            _httpClient = _factory.CreateClient();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(13)]
        [InlineData(8)]
        [InlineData(19)]
        [InlineData(1007)]
        public async Task GetAllCar_FromOneDealer_ReturnsOk(int dealerId)
        {
            var uri = $"{_uriStart}{dealerId}{_uriEnd}";

            var response = await _httpClient.GetAsync(uri);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(51)]
        [InlineData(250)]
        [InlineData(4)]
        public async Task GetAllCar_FromOneDealer_ReturnsNotFound(int dealerId)
        {
            var uri = $"{_uriStart}{dealerId}{_uriEnd}";

            var response = await _httpClient.GetAsync(uri);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(8, 12)]
        [InlineData(1, 2)]
        public async Task GetCar_FromDealerByCarId_ReturnsOk(int dealerId, int carId)
        {
            var uri = $"{_uriStart}{dealerId}{_uriEnd}/{carId}";

            var response = await _httpClient.GetAsync(uri);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}