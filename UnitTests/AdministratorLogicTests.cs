using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Entities.Models;
using Repository.AdministratorRepository;
using BussinessLogic;
using AutoMapper;
using Xunit;
using Moq;
using BussinessLogic.Exceptions;

namespace UnitTests
{
    public class AdministratorLogicTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IUserManipulationLogic> userManipulationMock;
        private Mock<ICustomerRepositoryForAdministrator> administratorCustomerRepositoryMock;
        private Mock<IAdministratorMessageRepositoryForAdministrator> administratorMessageRepositoryMock;
        private Mock<ICallRepositoryForAdministrator> administratorCallRepositoryMock;
        private Mock<IAdministratorManager> administratorManagerMock;
        private AdministratorLogic administratorLogic;

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

        private AdministratorMessage GetMessage()
        {
            return new AdministratorMessage() { CustomerId = 2, SendingTime = new DateTime(2022, 1, 23, 10, 11, 30) };
        }


        public AdministratorLogicTests()
        {
            mapperMock = new Mock<IMapper>();
            userManipulationMock = new Mock<IUserManipulationLogic>();

            administratorCustomerRepositoryMock = new Mock<ICustomerRepositoryForAdministrator>();
            administratorMessageRepositoryMock = new Mock<IAdministratorMessageRepositoryForAdministrator>();
            administratorCallRepositoryMock = new Mock<ICallRepositoryForAdministrator>();

            administratorManagerMock = new Mock<IAdministratorManager>();

            administratorLogic = new AdministratorLogic(administratorManagerMock.Object, userManipulationMock.Object, mapperMock.Object);

            userManipulationMock.Setup(m => m.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), "Customer")).ReturnsAsync(new User(It.IsAny<string>())).Verifiable();

            CreateMapMock<CustomerForCreateInAdministratorDto, Customer>(mapperMock);
            CreateMapMock<CustomerForUpdateInAdministratorDto, Customer>(mapperMock);
            CreateMapMock<Customer, CustomerForReadInAdministratorDto>(mapperMock);
            CreateMapMock<Call, CallForReadInAdministratorDto>(mapperMock);


            administratorCustomerRepositoryMock.Setup(m => m.AddCustomerAsync(It.IsAny<Customer>())).Verifiable();
            administratorCustomerRepositoryMock.Setup(m => m.GetCustomerAsync(It.IsAny<int>(), It.IsAny<bool>())).Returns((int id, bool tc) => Task.FromResult(GetCustomer(id)));
            //administratorCustomerRepositoryMock.Setup(m => m.GetUnblockedCustomerAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(GetCustomer(id)));
            administratorCustomerRepositoryMock.Setup(m => m.UpdateCustomer(It.IsAny<Customer>())).Verifiable();
            administratorCustomerRepositoryMock.Setup(m => m.FindCustomerByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(GetCustomer(0)));

            administratorCallRepositoryMock.Setup(m => m.GetCustomerCallsAsync(It.IsAny<int>(), It.IsAny<CallParameters>())).Returns((int id, CallParameters p) => Task.FromResult((id >= 1 && id <= 6) ? GetCalls() : null));
            administratorCallRepositoryMock.Setup(m => m.GetCallAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(GetCall(id)));

            administratorMessageRepositoryMock.Setup(m => m.GetCustomerWarningMessagesFromTimeAsync(It.IsAny<int>(), It.IsAny<DateTime>())).Returns((int id, DateTime dt) => Task.FromResult((id >= 1 && id <= 6) ? GetMessages() : null));


            administratorManagerMock.Setup(m => m.Customers).Returns(administratorCustomerRepositoryMock.Object);
            administratorManagerMock.Setup(m => m.Calls).Returns(administratorCallRepositoryMock.Object);
            administratorManagerMock.Setup(m => m.Messages).Returns(administratorMessageRepositoryMock.Object);
        }


        [Fact]
        public async Task CreateNewCustomer_CreateCustomerTest()
        {
            //Arrange
            var customerDto = new CustomerForCreateInAdministratorDto();
            customerDto.Login = "iiii";
            customerDto.Password = "eeee2eeee2eeee";
            customerDto.Name = "Arnold";
            customerDto.Surname = "Golh";
            customerDto.Patronymic = "Petrovich";
            customerDto.PhoneNumber = "375293618291";
            customerDto.Address = "Longon";
            customerDto.IsPhoneNumberHidedStr = "false";


            //Act
            await administratorLogic.CreateCustomerAsync(customerDto);

            //Assert
            userManipulationMock.Verify(m => m.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            administratorCustomerRepositoryMock.Verify(m => m.AddCustomerAsync(It.IsAny<Customer>()));
        }


        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(100)]
        public async Task GetInvalidCustomer_GetCustomerInfoTest(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var customer = await administratorLogic.GetCustomerInfoAsync(customerId);
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
        [InlineData(4)]
        [InlineData(6)]
        public async Task GetValidCustomer_GetCustomerInfoTest(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var customer = await administratorLogic.GetCustomerInfoAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        public async Task InvalidCustomer_TimePastsFromLastWarnMessageTest(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var time = await administratorLogic.TimePastsFromLastWarnMessageAsync(customerId);
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
        [InlineData(6)]
        public async Task ValidCustomer_TimePastsFromLastWarnMessageTest(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var time = await administratorLogic.TimePastsFromLastWarnMessageAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(100)]
        public async Task GetInvalidCall_GetCallInfoTest(int callId)
        {
            //Arrange
            CallForReadInAdministratorDto call = null;

            //Act
            try
            {
                call = await administratorLogic.GetCallInfoAsync(callId);
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
        [InlineData(4)]
        [InlineData(6)]
        public async Task GetValidCall_GetCallInfoTest(int callId)
        {
            //Arrange
            CallForReadInAdministratorDto call = null;

            //Act
            try
            {
                call = await administratorLogic.GetCallInfoAsync(callId);
            }
            //Assert
            catch (CallDoesntExistException ex)
            {
                Assert.IsNotType<CallDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(call);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_GetAdministratorMessagesByCustomerIdAsync(int customerId)
        {
            //Arrange
            var parameters = new AdministratorMessageParameters();
            parameters.customerId = customerId;
            parameters.PageNumber = 1;

            //Act
            try
            {
                var messages = await administratorLogic.GetAdministratorMessagesByCustomerIdAsync(customerId, parameters);
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
        [InlineData(2)]
        [InlineData(6)]
        public async Task ValidCustomer_GetAdministratorMessagesByCustomerIdAsync(int customerId)
        {
            //Arrange
            IEnumerable<AdministratorMessageForReadInAdministratorDto> messages = null;
            var parameters = new AdministratorMessageParameters();
            parameters.customerId = customerId;
            parameters.PageNumber = 1;


            //Act
            try
            {
                messages = await administratorLogic.GetAdministratorMessagesByCustomerIdAsync(customerId, parameters);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                Assert.True(true, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(messages);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_SendMessageAsync(int customerId)
        {
            //Arrange
            var messageDto = new AdministratorMessageForCreateInAdministratorDto();

            //Act
            try
            {
                await administratorLogic.SendMessageAsync(customerId, messageDto);
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
        [InlineData(2)]
        [InlineData(3)]
        public async Task CustomerWasBlockedOrLittleTimeHadPassed_SendMessageAsync(int customerId)
        {
            //Arrange
            var messageDto = new AdministratorMessageForCreateInAdministratorDto();

            //Act
            try
            {
                await administratorLogic.SendMessageAsync(customerId, messageDto);
            }
            //Assert
            catch (InvalidOperationException ex)
            {
                Assert.IsType<InvalidOperationException>(ex);
                Assert.True(true, ex.Message);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(2)]
        public async Task CustomerIsAlreadyBlocked_BlockCustomerAsync(int customerId)
        {
            //Arrange

            //Act
            try
            {
                await administratorLogic.BlockCustomerAsync(customerId);
            }
            //Assert
            catch (InvalidOperationException ex)
            {
                Assert.IsType<InvalidOperationException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_BlockCustomerAsync(int customerId)
        {
            //Arrange

            //Act
            try
            {
                await administratorLogic.BlockCustomerAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(true);
        }

        [Theory]
        [InlineData(3)]
        public async Task CustomerWasntBlocked_BlockCustomerAsync(int customerId)
        {
            //Arrange

            //Act
            try
            {
                await administratorLogic.BlockCustomerAsync(customerId);
            }
            //Assert
            catch (InvalidOperationException ex)
            {
                Assert.IsNotType<InvalidOperationException>(ex);
                return;
            }
            Assert.True(true);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_UpdateCustomerAsync(int customerId)
        {
            //Arrange
            var customerDto = new CustomerForUpdateInAdministratorDto();
            customerDto.IsBlocked = false;
            customerDto.PhoneNumber = "375330990919";


            //Act
            try
            {
                await administratorLogic.UpdateCustomerAsync(customerId, customerDto);
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
        [InlineData(4)]
        [InlineData(6)]
        public async Task ValidCustomer_UpdateCustomerAsync(int customerId)
        {
            //Arrange
            var customerDto = new CustomerForUpdateInAdministratorDto();
            customerDto.IsBlocked = false;
            customerDto.PhoneNumber = "375330990919";


            //Act
            try
            {
                await administratorLogic.UpdateCustomerAsync(customerId, customerDto);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            administratorCustomerRepositoryMock.Verify(m => m.UpdateCustomer(It.IsAny<Customer>()), Times.AtLeastOnce);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_DeleteCustomerAsync(int customerId)
        {
            //Arrange


            //Act
            try
            {
                await administratorLogic.DeleteCustomerAsync(customerId);
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
        [InlineData(4)]
        [InlineData(6)]
        public async Task ValidCustomer_DeleteCustomerAsync(int customerId)
        {
            //Arrange


            //Act
            try
            {
                await administratorLogic.DeleteCustomerAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            administratorCustomerRepositoryMock.Verify(m => m.DeleteCustomerByIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Theory]
        [InlineData("375330919381")]
        public async Task CheckPhoneNumber_CheckPhoneNumberForExistenceAsync(string phoneNumber)
        {
            //Arrange
            bool status = true;

            //Act
            status = await administratorLogic.CheckPhoneNumberForExistenceAsync(phoneNumber);

            //Assert
            Assert.False(status);
        }

        [Theory]
        [InlineData(1, "37529239103318")]
        [InlineData(6, "37529239103318")]
        public async Task ValidCustomer_TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber)
        {
            //Arrange
            bool status = false;


            //Act
            try
            {
                status = await administratorLogic.TryToSetNewPhoneNumberAsync(customerId, phoneNumber);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }

            Assert.True(status);
        }

        [Theory]
        [InlineData(0, "37529239103318")]
        [InlineData(7, "37529239103318")]
        public async Task InvalidCustomer_TryToSetNewPhoneNumberAsync(int customerId, string phoneNumber)
        {
            //Arrange
            bool status = false;


            //Act
            try
            {
                status = await administratorLogic.TryToSetNewPhoneNumberAsync(customerId, phoneNumber);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public async Task GetCustomersAsync()
        {
            //Arrange
            var parameters = new CustomerParameters();

            //Act
            var customers = await administratorLogic.GetCustomersInfoAsync(parameters);

            //Assert
            Assert.NotNull(customers);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(6)]
        public async Task ValidCustomer_GetCustomerCallsAsync(int customerId)
        {
            //Arrange
            var parameters = new CallParameters();
            IEnumerable<CallForReadInAdministratorDto> customers = null;


            //Act
            try
            {
                customers = await administratorLogic.GetCustomerCallsInfoAsync(customerId, parameters);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(customers);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_GetCustomerCallsAsync(int customerId)
        {
            //Arrange
            var parameters = new CallParameters();


            //Act
            try
            {
                var customers = await administratorLogic.GetCustomerCallsInfoAsync(customerId, parameters);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Fact]
        public async Task GetCallsAsync()
        {
            //Arrange
            var parameters = new CallParameters();

            //Act
            var calls = await administratorLogic.GetCallsInfoAsync(parameters);

            //Assert
            Assert.NotNull(calls);
        }
    }
}
