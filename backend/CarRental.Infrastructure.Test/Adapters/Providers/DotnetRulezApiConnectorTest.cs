using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.Out;
using CarRental.Infrastructure.Adapters.Providers;
using CarRental.Infrastructure.Util;
using Moq;
using Moq.Protected;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace CarRental.Infrastructure.Test.Adapters.Providers
{
    public class DotnetRulezApiConnectorTest
    {
        private readonly Mock<HttpMessageHandler> _messageHandler = new Mock<HttpMessageHandler>();
        private readonly ICarProvider _provider;

        public DotnetRulezApiConnectorTest()
        {
            var client = new HttpClient(_messageHandler.Object);
            var factory = new Mock<IHttpClientFactory>();
            factory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(client);
            _provider = new DotnetRulezApiConnector(factory.Object,
                new CarProviderConfig()
                {
                    BaseUrl = "http://example.com",
                    Config = new Dictionary<string, string>() { {"ApiKey", "123"} }
                },
                (new Mock<IConfiguration>()).Object);
        }
        
        [Fact]
        public async Task ShouldQueryAndFilterListOfCars()
        {
            var cars = new DotnetRulezApiConnector.DNZCarsListResponse()
            {
                Count = 1,
                Cars = new List<DotnetRulezApiConnector.DNZCarDetails>()
                {
                    new()
                    {
                        Id = 1,
                        Brand = "ABC",
                        Model = "DEF",
                        ProductionYear = 2000,
                        Capacity = 4,
                        Category = "Big",
                        HorsePower = 1,
                        ProviderCompany = "DNR"
                    },
                    new()
                    {
                        Id = 1,
                        Brand = "AA",
                        Model = "BB",
                        ProductionYear = 2000,
                        Capacity = 4,
                        Category = "Big",
                        HorsePower = 1,
                        ProviderCompany = "DNR"
                    },
                }
            };
            _messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                    {StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(cars))});
            var filters = new CarListFilter {Brands = new[] {"AA"}};

            var response = await _provider.GetCarsAsync(filters);

            Assert.NotNull(response.Data);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task ShouldSendCheckPriceRequestAndParseResult()
        {
            var price = new DotnetRulezApiConnector.DNZCarPrice()
            {
                Currency = "PLN",
                Price = 10,
                ExpiredAt = new DateTime(),
                GeneratedAt = new DateTime(),
                Id = 1
            };
            _messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                    {StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(price))});
            
            var response = await _provider.CheckPrice("1", 1, new UserDetails());

            Assert.NotNull(response.Data);
            Assert.Equal("1", response.Data.Id);
        }

        [Fact]
        public async Task ShouldSendBookCarRequestAndParseResult()
        {
            var rent = new DotnetRulezApiConnector.DNZRentCarResponse()
            {
                RentId = 2
            };
            _messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                    {StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(rent))});
            
            var response = await _provider.TryBookCar("1", new CarRentRequest() { PriceId = "1"});

            Assert.NotNull(response.Data);
            Assert.Equal("2", response.Data.RentId);
        }
        
        [Fact]
        public async Task ShouldSendReturnRequest()
        {
            _messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                    {StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(new { Message = "OK" }))});
            
            var response = await _provider.TryReturnCar("1");

            Assert.Null(response.Error);
        }
    }
}