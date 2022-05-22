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
using Repository.GuestRepository;
using Entities.Models;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;

namespace UnitTests
{
    public class GuestLogicTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<ICustomerRepositoryForGuest> guestCustomerRepositoryMock;
        private Mock<IGuestManager> guestManagerMock;
        private GuestLogic guestLogic;

        private void CreateMapMock<ResourseType, DestinationType>(Mock<IMapper> mapperMock) where DestinationType : new()
        {
            mapperMock.Setup(m => m.Map<DestinationType>(It.IsAny<ResourseType>())).Returns(new DestinationType());
        }

        private IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>()
            {
                new Customer() {Id = 1, IsBlocked = false, IsPhoneNumberHided = false},
                new Customer() {Id = 2, IsBlocked = true, IsPhoneNumberHided = false},
                new Customer() {Id = 3, IsBlocked = false, IsPhoneNumberHided = true},
                new Customer() {Id = 4, IsBlocked = false, IsPhoneNumberHided = false},
                new Customer() {Id = 5, IsBlocked = false, IsPhoneNumberHided = true},
                new Customer() {Id = 6, IsBlocked = false, IsPhoneNumberHided = false}
            };
        }

        private Customer GetCustomer(int customerId)
        {
            return GetCustomers().Where(c => c.Id.Equals(customerId) && !c.IsBlocked && !c.IsPhoneNumberHided).FirstOrDefault();
        }

        public GuestLogicTests()
        {
            mapperMock = new Mock<IMapper>();

            guestCustomerRepositoryMock = new Mock<ICustomerRepositoryForGuest>();

            guestManagerMock = new Mock<IGuestManager>();

            guestLogic = new GuestLogic(guestManagerMock.Object, mapperMock.Object);

            CreateMapMock<Customer, CustomerForReadInGuestDto>(mapperMock);

            guestCustomerRepositoryMock.Setup(m => m.GetCustomerInfoAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(GetCustomer(id)));
            guestCustomerRepositoryMock.Setup(m => m.GetCustomersAsync(It.IsAny<CustomerParameters>())).Returns((CustomerParameters p) => Task.FromResult(GetCustomers()));

            guestManagerMock.Setup(m => m.Customers).Returns(guestCustomerRepositoryMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(6)]
        public async Task ValidCustomer_GetCustomerInfoAsync(int customerId)
        {
            //Arrange
            CustomerForReadInGuestDto customer;

            //Act
            try
            {
                customer = await guestLogic.GetCustomerInfoAsync(customerId);
            }
            //Assert
            catch (CustomerDoesntExistException ex)
            {
                Assert.IsNotType<CustomerDoesntExistException>(ex);
                return;
            }
            Assert.NotNull(customer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(8)]
        public async Task InvalidCustomer_GetCustomerInfoAsync(int customerId)
        {
            //Arrange

            //Act
            try
            {
                var customer = await guestLogic.GetCustomerInfoAsync(customerId);
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
            var customer = await guestLogic.GetCustomersAsync(parameters);

            //Assert
            Assert.NotNull(customer);
        }
    }
}
