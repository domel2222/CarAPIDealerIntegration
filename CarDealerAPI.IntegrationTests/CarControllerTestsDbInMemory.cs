using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using CarDealerAPI.IntegrationTests.Helpers;
using CarDealerAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CarDealerAPI.IntegrationTests
{
    public class CarControllerTestsDbInMemory : IClassFixture<WebApplicationFactory<Program>>
    {
        private string _urlStart = "api/Dealer/";
        private string _urlEnd = "/car";
        private WebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;

        public CarControllerTestsDbInMemory(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            var dbContextOption = services.SingleOrDefault(ser => ser.ServiceType == typeof(DbContextOptions<DealerDbContext>));

                            services.Remove(dbContextOption);

                            services.AddSingleton<IPolicyEvaluator, FakePolicyevaluator>();

                            services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                            services.AddDbContext<DealerDbContext>(option => option.UseInMemoryDatabase("DealerDb"));

                            var builder = new DbContextOptionsBuilder<DealerDbContext>();
                            builder.UseInMemoryDatabase("TestDbDealer");
                        });
                    });

            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateCar_ForDealerByDealreId_ValidModel_RetrunsCreated()
        {
            var dealerModel = new DealerCreateDTO()
            {
                DealerName = "TestDealerTesla",
                City = "Sigapour",
                Street = "Pingpong"
            };

            var httpContent = dealerModel.ToJsonHttpContent();

            var dealerRespone = await _httpClient.PostAsync(_urlStart, httpContent);

            Int32.TryParse(dealerRespone.Headers.Location.ToString().Split("/").Last(), out int dealerId);

            var carModel = new CarCreateDTO()
            {
                NameMark = "Porsche",
                Model = "Cayman GT4",
                Price = 100.00M,
                DealerId = dealerId
            };

            var httpContentCar = carModel.ToJsonHttpContent();

            var url = $"{_urlStart}{dealerId}{_urlEnd}";

            var carResponse = await _httpClient.PostAsync(url, httpContentCar);

            carResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(11)]
        [InlineData(65)]
        public async Task CreateCar_ForDealerByDealreId_NotExistDealer_RetrunsNotFound(int dealerId)
        {
            var carModel = new CarCreateDTO()
            {
                NameMark = "Fiat",
                Model = "Punto",
                Price = 220.00M,
                DealerId = 5
            };

            var httpContentCar = carModel.ToJsonHttpContent();

            var url = $"{_urlStart}{dealerId}{_urlEnd}";

            var carResponse = await _httpClient.PostAsync(url, httpContentCar);

            carResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task DeleteAllCars_FromDealerById_ReturnNoContent()
        {
            SeedDealerToDb();

            var dealerId = 1;
            var url = $"{_urlStart}{dealerId}{_urlEnd}";

            var response = await _httpClient.DeleteAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        private int SeedDealerToDb(Dealer model = null)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<DealerDbContext>();

            if (model == null)
            {
                model = new Dealer()
                {
                    CreatedById = 1,
                    DealerName = "Bonzo Speed",
                    Category = "Sports",
                    Description =
                    "Every cloud has a silver lining",
                    ContactEmail = "contact@bonzopoland.com",
                    TestDrive = true,

                    Cars = new List<Car>()
                    {
                        new Car()
                            {
                                NameMark = "Hyundai",
                                Model = "i30",
                                Price = 15.00M
                            },

                        new Car()
                            {
                                NameMark = "Renault",
                                Model = "R.S. Line",
                                Price = 115.00M
                            },
                    },

                    Address = new Address()
                    {
                        City = "Bydgoszcz",
                        Country = "Poland",
                        Street = "Wiejska 15",
                    }
                };
            }

            dbContext.Dealers.Add(model);
            dbContext.SaveChanges();

            return model.Id;
        }
    }
}