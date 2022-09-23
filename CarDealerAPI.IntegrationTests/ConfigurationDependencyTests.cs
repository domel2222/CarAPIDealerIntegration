using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CarDealerAPI.IntegrationTests
{
    public class ConfigurationDependencyTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly List<Type> _controllerTypes;
        private readonly WebApplicationFactory<Program> _webApplicationFactory;

        public ConfigurationDependencyTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _controllerTypes = typeof(Program)
               .Assembly
               .GetTypes()
               .Where(c => c.IsSubclassOf(typeof(ControllerBase)))
               .ToList();

            _webApplicationFactory = webApplicationFactory.WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            _controllerTypes.ForEach(c => services.AddScoped(c));
                        });
                    });
        }

        [Fact]
        public void ConfigurationServices_ForControlers_HasAllDependencies()
        {
            var scopeFactory = _webApplicationFactory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            //var controller = scope.ServiceProvider.GetService<AccountController>();
            _controllerTypes.ForEach(c =>
            {
                var controller = scope.ServiceProvider.GetService(c);
                controller.Should().NotBeNull();
            });
        }
    }
}