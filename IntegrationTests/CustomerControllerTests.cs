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
    public class CustomerControllerTests
    {
        private HttpClient _client;
        private readonly string jwtToken;

        public CustomerControllerTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            _client = server.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/login?Login=kaktus&Password=pony1pony2");
            var response = _client.SendAsync(request).Result;
            jwtToken = response.Content.ReadAsStringAsync().Result;
        }

        private async Task<string> AuthorizeAsync(string userData)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/login?{userData}");
            var response = await _client.SendAsync(request);
            string s = await response.Content.ReadAsStringAsync();
            return s;
        }

        [Theory]
        [InlineData("POST", "Login=kaktus&Password=pony1pony2")]
        public async Task GetAuthorizationToken(string method, string userData)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/login?{userData}");

            //Act
            var response = await _client.SendAsync(request);
            //string s = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "PageNumber=1&PageSize=50")]
        [InlineData("GET", "PageNumber=1")]
        [InlineData("GET", "PageNumber=1&Name=Derek")]
        public async Task ValidParameters_GetCustomersTest(string method, string parameters)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/customer-profile/customers?{parameters}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

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
            var request = new HttpRequestMessage(new HttpMethod(method), $"/customer-profile/customers/customer/{customerId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        public async Task Valid_GetInfoTest(string method)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/customer-profile/info");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            //Assert.True(false, await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
