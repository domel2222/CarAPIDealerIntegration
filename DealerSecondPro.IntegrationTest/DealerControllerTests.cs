using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DealerSecondPro.IntegrationTest
{
    public class DealerControllerTests
    {
        [Fact]

        public async Task GetAll_WithQueryParameters_ReturnOkResult()
        {
            var factory = new WebApplicationFactory<Program>();
            var clinet = factory.CreateClient();


            var url = "/api/Dealer?PageSize=5&PageNumber=2";
            //var response = await _httpClient.GetAsync(url);
            var response = await clinet.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}