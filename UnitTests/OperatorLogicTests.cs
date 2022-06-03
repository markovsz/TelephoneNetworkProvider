using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Xunit;
using Moq;
using BussinessLogic;
using BussinessLogic.Exceptions;
using Repository.OperatorRepository;
using Entities.Models;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
;

namespace UnitTests
{
    public class OperatorLogicTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<ICustomerRepositoryForOperator> operatorCustomerRepositoryMock;
        private Mock<ICallRepositoryForOperator> operatorCallRepositoryMock;
        private Mock<IOperatorManager> operatorManagerMock;
        private OperatorLogic operatorLogic;

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

        private Customer GetUnblockedCustomer(int customerId)
        {
            return GetCustomers().Where(c => c.Id.Equals(customerId) && !c.IsBlocked).FirstOrDefault();
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

        public OperatorLogicTests()
        {
            mapperMock = new Mock<IMapper>();

            operatorCustomerRepositoryMock = new Mock<ICustomerRepositoryForOperator>();
            operatorCallRepositoryMock = new Mock<ICallRepositoryForOperator>();

            operatorManagerMock = new Mock<IOperatorManager>();

            operatorLogic = new OperatorLogic(operatorManagerMock.Object, mapperMock.Object);

            CreateMapMock<Customer, CustomerForReadInOperatorDto>(mapperMock);
            CreateMapMock<Call, CallForReadInOperatorDto>(mapperMock);
            //CreateMapMock<CallForCreateInOperatorDto, Call>(mapperMock);
            mapperMock.Setup(m => m.Map<Call>(It.IsAny<CallForCreateInOperatorDto>())).Returns((CallForCreateInOperatorDto cd) => new Call() { CallerId = cd.CallerId, CalledById = cd.CalledById });


            operatorCustomerRepositoryMock.Setup(m => m.GetUnblockedCustomerAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(GetUnblockedCustomer(id)));
            operatorCustomerRepositoryMock.Setup(m => m.GetCustomerAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(GetCustomer(id)));
            operatorCustomerRepositoryMock.Setup(m => m.GetCustomersAsync(It.IsAny<CustomerParameters>())).Returns((CustomerParameters p) => Task.FromResult(GetCustomers()));
            
            operatorCallRepositoryMock.Setup(m => m.CreateCallAsync(It.IsAny<Call>())).Verifiable();
            operatorCallRepositoryMock.Setup(m => m.DeleteCallByIdAsync(It.IsAny<int>())).Verifiable();
            operatorCallRepositoryMock.Setup(m => m.GetCustomerCallsAsync(It.IsAny<int>(), It.IsAny<CallParameters>())).Returns((int id, CallParameters p) => Task.FromResult(id >= 1 && id <= 6 ? GetCalls() : null));
            operatorCallRepositoryMock.Setup(m => m.GetCallsAsync(It.IsAny<CallParameters>())).Returns((CallParameters p) => Task.FromResult(GetCalls()));
            operatorCallRepositoryMock.Setup(m => m.GetCallAsync(It.IsAny<int>(), It.IsAny<bool>())).Returns((int id, bool tc) => Task.FromResult(GetCall(id)));

            operatorManagerMock.Setup(m => m.Customers).Returns(operatorCustomerRepositoryMock.Object);
            operatorManagerMock.Setup(m => m.Calls).Returns(operatorCallRepositoryMock.Object);
        }

        [Theory]
        [InlineData(-1, 3)]
        [InlineData(-1, -3)]
        [InlineData(-1, 0)]
        [InlineData(1, 0)]
        [InlineData(1, 8)]
        [InlineData(6, 7)]
        [InlineData(0, 0)]
        [InlineData(2, 6)]
        [InlineData(7, 7)]
        public async Task InvalidCustomer_CreateCallAsync(int callerId, int calledById)
        {
            //Arrange
            var callDto = new CallForCreateInOperatorDto();
            callDto.CallerId = callerId;
            callDto.CalledById = calledById;

            //Act
            try
            {
                await operatorLogic.CreateCallAsync(callDto);
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
        [InlineData(1, 3)]
        [InlineData(1, 5)]
        [InlineData(1, 6)]
        [InlineData(3, 3)]
        [InlineData(6, 6)]
        [InlineData(1, 1)]
        public async Task ValidCustomer_CreateCallAsync(int callerId, int calledById)
        {
            //Arrange
            var callDto = new CallForCreateInOperatorDto();
            callDto.CallerId = callerId;
            callDto.CalledById = calledById;

            //Act
            try
            {
                await operatorLogic.CreateCallAsync(callDto);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            operatorCallRepositoryMock.Verify(m => m.CreateCallAsync(It.IsAny<Call>()));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-13)]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(60)]
        public async Task InvalidCustomer_DeleteCallAsync(int callId)
        {
            //Arrange

            //Act
            try
            {
                await operatorLogic.DeleteCallAsync(callId);
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
        public async Task ValidCustomer_DeleteCallAsync(int callId)
        {
            //Arrange

            //Act
            try
            {
                await operatorLogic.DeleteCallAsync(callId);
            }
            //Assert
            catch (CallDoesntExistException ex)
            {
                Assert.IsNotType<CallDoesntExistException>(ex);
                return;
            }
            operatorCallRepositoryMock.Verify(m => m.DeleteCallByIdAsync(It.IsAny<int>()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCustomer_GetCustomerInfoAsync(int customerId)
        {
            //Arrange
            var customerDto = new CustomerForReadInOperatorDto();

            //Act
            try
            {
                customerDto = await operatorLogic.GetCustomerInfoAsync(customerId); 
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
        public async Task InvalidCustomer_GetCustomerInfoAsync(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var customerDto = await operatorLogic.GetCustomerInfoAsync(customerId);
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
            IEnumerable<CustomerForReadInOperatorDto> customers;

            //Act
            customers = await operatorLogic.GetCustomersInfoAsync(parameters);

            //Assert
            Assert.NotNull(customers);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCall_GetCallAsync(int callId)
        {
            //Arrange
            CallForReadInOperatorDto callDto;

            //Act
            try
            {
                callDto = await operatorLogic.GetCallInfoAsync(callId);
            }
            //Assert
            catch (CallDoesntExistException ex)
            {
                Assert.IsNotType<CallDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(callDto);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCall_GetCallAsync(int callId)
        {
            //Arrange

            //Act
            try
            {
                var callDto = await operatorLogic.GetCallInfoAsync(callId);
            }
            //Assert
            catch (CallDoesntExistException ex)
            {
                Assert.IsType<CallDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }

        [Fact]
        public async Task GetCallsAsync()
        {
            //Arrange
            var parameters = new CallParameters();
            IEnumerable<CallForReadInOperatorDto> calls;

            //Act
            calls = await operatorLogic.GetCallsInfoAsync(parameters);

            //Assert
            Assert.NotNull(calls);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task ValidCustomer_GetCustomerCallsAsync(int customerId)
        {
            //Arrange
            var parameters = new CallParameters();
            IEnumerable<CallForReadInOperatorDto> calls;

            //Act
            try
            {
                 calls = await operatorLogic.GetCustomerCallsInfoAsync(customerId, parameters);
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
        public async Task InvalidCustomer_GetCustomerCallsAsync(int customerId)
        {
            //Arrange
            var parameters = new CallParameters();

            //Act
            try
            {
                var calls = await operatorLogic.GetCustomerCallsInfoAsync(customerId, parameters);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.True(false);
        }
    }
}
