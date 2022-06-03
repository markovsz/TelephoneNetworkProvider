using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Repository.CustomerRepository;
using BussinessLogic;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using BussinessLogic.Exceptions;

namespace UnitTests
{
    public class CustomerLogicTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<ICustomerRepositoryForCustomer> customerCustomerRepositoryMock;
        private Mock<IAdministratorMessageRepositoryForCustomer> administratorMessageRepositoryMock;
        private Mock<ICallRepositoryForCustomer> customerCallRepositoryMock;
        private Mock<ICustomerManager> customerManagerMock;
        private CustomerLogic customerLogic;

        private void CreateMapMock<ResourseType, DestinationType>(Mock<IMapper> mapperMock) where DestinationType : new()
        {
            mapperMock.Setup(m => m.Map<DestinationType>(It.IsAny<ResourseType>())).Returns(new DestinationType());
        }

        private IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>()
            {
                new Customer() {Id = 1, IsBlocked = false},
                new Customer() {Id = 2, IsBlocked = true},
                new Customer() {Id = 3, IsBlocked = false},
                new Customer() {Id = 4, IsBlocked = false},
                new Customer() {Id = 5, IsBlocked = false},
                new Customer() {Id = 6, IsBlocked = false}
            };
        }

        private Customer GetCustomer(int customerId)
        {
            return GetCustomers().Where(c => c.Id.Equals(customerId)).FirstOrDefault();
        }

        private IEnumerable<Call> GetCalls()
        {
            return new List<Call>()
            {
                new Call() {Id = 1, CallerId = 1, CalledById = 6},
                new Call() {Id = 2, CallerId = 4, CalledById = 3},
                new Call() {Id = 3, CallerId = 5, CalledById = 1},
                new Call() {Id = 4, CallerId = 2, CalledById = 6},
                new Call() {Id = 5, CallerId = 5, CalledById = 4},
                new Call() {Id = 6, CallerId = 3, CalledById = 2}
            };
        }

        private Call GetCall(int id)
        {
            return GetCalls().Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        private IEnumerable<AdministratorMessage> GetMessages()
        {
            return new List<AdministratorMessage>()
            {
                new AdministratorMessage() {Id = 1, CustomerId = 2, SendingTime = new DateTime(2022,1,23,10,11,30)},
                new AdministratorMessage() {Id = 2, CustomerId = 2, SendingTime = new DateTime(2022,1,24,12,00,29)},
                new AdministratorMessage() {Id = 3, CustomerId = 2, SendingTime = new DateTime(2022,1,25,16,37,17)},
                new AdministratorMessage() {Id = 4, CustomerId = 3, SendingTime = DateTime.Now}
            };
        }

        private AdministratorMessage GetMessage(int id)
        {
            return GetMessages().Where(m => m.Id.Equals(id)).FirstOrDefault();
        }

        public CustomerLogicTests()
        {
            mapperMock = new Mock<IMapper>();

            customerCustomerRepositoryMock = new Mock<ICustomerRepositoryForCustomer>();
            administratorMessageRepositoryMock = new Mock<IAdministratorMessageRepositoryForCustomer>();
            customerCallRepositoryMock = new Mock<ICallRepositoryForCustomer>();

            customerManagerMock = new Mock<ICustomerManager>();

            customerLogic = new CustomerLogic(customerManagerMock.Object, mapperMock.Object);

            CreateMapMock<CustomerForUpdateInCustomerDto, Customer>(mapperMock);
            CreateMapMock<Customer, CustomerForReadInCustomerDto>(mapperMock);
            CreateMapMock<Call, CallForReadInCustomerDto>(mapperMock);


            customerCustomerRepositoryMock.Setup(m => m.UpdateCustomer(It.IsAny<Customer>())).Verifiable();
            customerCustomerRepositoryMock.Setup(m => m.GetCustomerAsync(It.IsAny<int>(), It.IsAny<bool>())).Returns((int id, bool tc) => Task.FromResult(GetCustomer(id)));
            customerCustomerRepositoryMock.Setup(m => m.GetCustomersAsync(It.IsAny<CustomerParameters>())).Returns((CustomerParameters p) => Task.FromResult(GetCustomers()));
            //customerCustomerRepositoryMock.Setup(m => m.DeleteCustomerByUserIdAsync(It.IsAny<int>()));

            customerCallRepositoryMock.Setup(m => m.GetCallsAsync(It.IsAny<int>(), It.IsAny<CallParameters>())).Returns((int id, CallParameters p) => Task.FromResult((id >= 1 && id <= 6) ? GetCalls() : null));
            customerCallRepositoryMock.Setup(m => m.GetCallAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(GetCall(id)));

            administratorMessageRepositoryMock.Setup(m => m.GetMessagesAsync(It.IsAny<int>(), It.IsAny<AdministratorMessageParameters>())).Returns((int id, AdministratorMessageParameters p) => Task.FromResult((id >= 1 && id <= 6) ? GetMessages() : null));


            customerManagerMock.Setup(m => m.Customers).Returns(customerCustomerRepositoryMock.Object);
            customerManagerMock.Setup(m => m.Calls).Returns(customerCallRepositoryMock.Object);
            customerManagerMock.Setup(m => m.Messages).Returns(administratorMessageRepositoryMock.Object);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_GetCustomerInfoAsyncTest(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var customerDto = await customerLogic.GetCustomerInfoAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCustomer_GetCustomerInfoAsyncTest(int customerId)
        {
            //Arrange
            CustomerForReadInCustomerDto customerDto;

            //Act
            try
            {
                customerDto = await customerLogic.GetCustomerInfoAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(customerDto);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCall_GetCallInfoAsyncTest(int callId)
        {
            //Arrange

            //Act
            try
            {
                var callDto = await customerLogic.GetCallInfoAsync(callId);
            }
            //Assert
            catch (CallDoesntExistException ex)
            {
                Assert.IsType<CallDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCall_GetCallInfoAsyncTest(int callId)
        {
            //Arrange
            CallForReadInCustomerDto callDto;

            //Act
            try
            {
                callDto = await customerLogic.GetCallInfoAsync(callId);
            }
            //Assert
            catch (CallDoesntExistException ex)
            {
                Assert.IsNotType<CallDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(callDto);
        }

        [Fact]
        public async Task GetCustomersAsyncTest()
        {
            //Arrange
            var parameters = new CustomerParameters();
            IEnumerable<CustomerForReadInCustomerDto> customers;

            //Act
            customers = await customerLogic.GetCustomersInfoAsync(parameters);

            //Assert
            Assert.NotNull(customers);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCustomer_GetCallsAsyncTest(int customerId)
        {
            //Arrange
            IEnumerable<CallForReadInCustomerDto> calls;
            var parameters = new CallParameters();

            //Act
            try
            {
                calls = await customerLogic.GetCallsInfoAsync(customerId, parameters);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(calls);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_GetCallsAsyncTest(int customerId)
        {
            //Arrange
            var parameters = new CallParameters();

            //Act
            try
            {
                var calls = await customerLogic.GetCallsInfoAsync(customerId, parameters);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCustomer_UpdateCustomerInfoTest(int customerId)
        {
            //Arrange
            var customerDto = new CustomerForUpdateInCustomerDto();

            //Act
            try
            {
                await customerLogic.UpdateCustomerInfo(customerId, customerDto);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }

            customerCustomerRepositoryMock.Verify(m => m.UpdateCustomer(It.IsAny<Customer>()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_UpdateCustomerInfoTest(int customerId)
        {
            //Arrange
            var customerDto = new CustomerForUpdateInCustomerDto();

            //Act
            try
            {
                await customerLogic.UpdateCustomerInfo(customerId, customerDto);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(7, 100)]
        [InlineData(8, 100)]
        public async Task InvalidCustomer_ReplenishTheBalanceAsync(int customerId, Decimal currency)
        {
            //Arrange
            //Act
            try
            {
                await customerLogic.ReplenishTheBalanceAsync(customerId, currency);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(1, -100)]
        [InlineData(1, -1)]
        [InlineData(3, 500000)]
        [InlineData(6, 50000.5)]
        public async Task InvalidCurrency_ReplenishTheBalanceAsync(int customerId, Decimal currency)
        {
            //Arrange
            //Act
            try
            {
                await customerLogic.ReplenishTheBalanceAsync(customerId, currency);
            }
            //Assert
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsType<ArgumentOutOfRangeException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(1, 1)]
        [InlineData(3, 5000)]
        [InlineData(6, 500.5)]
        [InlineData(6, 4999.9)]
        public async Task ValidCustomerAndCurrency_ReplenishTheBalanceAsync(int customerId, Decimal currency)
        {
            //Arrange
            //Act
            try
            {
                await customerLogic.ReplenishTheBalanceAsync(customerId, currency);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotType<ArgumentOutOfRangeException>(ex);
                return;
            }
            Assert.True(true);
        }
    }
}
