using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TelephoneNetworkProvider;
using Xunit;

namespace IntegrationTests
{
    public class GuestControllerTests
    {
        private HttpClient _client;
        private readonly string jwtToken;

        public GuestControllerTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET", "PageNumber=1&PageSize=50")]
        [InlineData("GET", "PageNumber=1")]
        [InlineData("GET", "PageNumber=1&Name=Derek")]
        public async Task ValidParameters_GetCustomersTest(string method, string parameters)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/guest-profile/customers?{parameters}");
            
            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 2)]
        [InlineData("GET", 4)]
        [InlineData("GET", 5)]
        public async Task ValidCustomer_GetCustomerInfoTest(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/guest-profile/customers/customer/{customerId}");
            
            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "Name=Derek")]
        [InlineData("GET", "PageNumber=1&PageSize=500")]
        [InlineData("GET", "PageSize=50")]
        [InlineData("GET", "PageNumbe=1")]
        public async Task InvalidParameters_GetCustomersTest(string method, string parameters)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/guest-profile/customers?{parameters}");

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 1)]
        [InlineData("GET", 0)]
        [InlineData("GET", -1)]
        public async Task InvalidCustomer_GetCustomerInfoTest(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/guest-profile/customers/customer/{customerId}");

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);//422 (Unprocessable entity)
        }
    }
}
