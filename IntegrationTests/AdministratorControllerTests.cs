using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TelephoneNetworkProvider;
using Xunit;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public class AdministratorControllerTests
    {
        private HttpClient _client;
        private readonly string jwtToken;

        public AdministratorControllerTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            _client = server.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/login?Login=iiii&Password=eeee2eeee2eeee");
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
        [InlineData("POST", "Login=iiii&Password=eeee2eeee2eeee")]
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
        public async Task GetCustomersTest(string method, string parameters)
        {
            //Arrange
            //string jwtToken = await AuthorizeAsync("Login=iiii&Password=eeee2eeee2eeee");
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers?{parameters}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            //response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 4)]
        public async Task GetCustomerInfoTest(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/{customerId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Theory]
        [InlineData("GET", 2)]
        public async Task GetCallInfoTest(string method, int callId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/calls/call/{callId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Theory]
        [InlineData("GET", "29999999999999999999")]
        public async Task TooLongCallId_GetCallInfoTest(string method, string callId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/calls/call/{callId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
