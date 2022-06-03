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
        public async Task ValidParameters_GetCustomersTest(string method, string parameters)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers?{parameters}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "PageNumber=-1&PageSize=50")]
        [InlineData("GET", "PageNumber=0&PageSize=50")]
        [InlineData("GET", "PageNumber=100000000&PageSize=50")]
        public async Task InvalidPageNumber_GetCustomersTest(string method, string parameters)
        {
            //Arrange
            //string jwtToken = await AuthorizeAsync("Login=iiii&Password=eeee2eeee2eeee");
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers?{parameters}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            //response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "PageNumber=1&PageSize=-1")]
        [InlineData("GET", "PageNumber=1&PageSize=0")]
        [InlineData("GET", "PageNumber=1&PageSize=500")]
        public async Task InvalidPageSize_GetCustomersTest(string method, string parameters)
        {
            //Arrange
            //string jwtToken = await AuthorizeAsync("Login=iiii&Password=eeee2eeee2eeee");
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers?{parameters}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            //response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 2)]
        [InlineData("GET", 4)]
        [InlineData("GET", 5)]
        public async Task ValidCustomer_GetCustomerInfoTest(string method, int customerId)
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
        [InlineData("GET", -1)]
        [InlineData("GET", 0)]
        [InlineData("GET", 1)]
        [InlineData("GET", 3)]
        [InlineData("GET", 6)]
        [InlineData("GET", 60)]
        public async Task InvalidCustomer_GetCustomerInfoTest(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/{customerId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 2)]
        public async Task InvalidCall_GetCallInfoTest(string method, int callId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/calls/call/{callId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

        [Theory]
        [InlineData("GET", "375297313127")]
        public async Task ValidPhoneNumber_CheckPhoneNumberForExistence(string method, string phoneNumber)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/phonenumber/check?phoneNumber={phoneNumber}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("true", content);
        }

        [Theory]
        [InlineData("GET", "375297313128")]
        public async Task InvalidPhoneNumber_CheckPhoneNumberForExistence(string method, string phoneNumber)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/phonenumber/check?phoneNumber={phoneNumber}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("false", content);
        }

        [Theory]
        [InlineData("GET", 2)]
        [InlineData("GET", 4)]
        [InlineData("GET", 5)]
        public async Task ValidCustomer_GetAdministratorMessagesByCustomerIdAsync(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/{customerId}/messages?PageNumber=1");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            //Assert.True(false, await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 0)]
        [InlineData("GET", -1)]
        [InlineData("GET", 9000)]
        public async Task InvalidCustomer_GetAdministratorMessagesByCustomerIdAsync(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/{customerId}/messages?PageNumber=1");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "l")]
        [InlineData("GET", "param=2")]
        [InlineData("GET", "2")]
        [InlineData("GET", "PageNumber=2")]
        public async Task InvalidParameters_GetAdministratorMessagesByCustomerIdAsync(string method, string parameters)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/2/messages?{parameters}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 2)]
        [InlineData("GET", 4)]
        [InlineData("GET", 5)]
        public async Task CustomerWithoutMessages_TimePastsFromLastWarnMessageAsync(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/{customerId}/time-pasts-from-last-warn-message");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);
            var res = await response.Content.ReadAsStringAsync();
            

            //Assert
            //Assert.Equal("", res);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", -1)]
        [InlineData("GET", 0)]
        [InlineData("GET", 1)]
        [InlineData("GET", 6)]
        [InlineData("GET", 60)]
        public async Task InvalidCustomer_TimePastsFromLastWarnMessageAsync(string method, int customerId)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/administrator-profile/customers/customer/{customerId}/time-pasts-from-last-warn-message");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            //Act
            var response = await _client.SendAsync(request);
            var res = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
