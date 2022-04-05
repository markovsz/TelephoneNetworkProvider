using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Repository;

namespace BussinessLogic
{
    public class CustomerLogic : GuestLogic, ICustomerLogic
    {
        private ICustomerManager _customerManager;
        private IMapper _mapper;
        private IGuestManager _guestManager;

        public CustomerLogic(ICustomerManager customerManager, IMapper mapper, IGuestManager guestManager)
            : base(guestManager)
        {
            _customerManager = customerManager;
            _mapper = mapper;
            _guestManager = guestManager;
        }

        public void UpdateCustomerInfo(CustomerForUpdateInCustomerDto customerDto)
        {

        }

        public void ReplenishTheBalance(Decimal currency)
        {

        }

        public IEnumerable<Call> GetCalls(CallParameters parameters)
        {
            return 
        }

        public CustomerForReadInCustomerDto GetCustomerInfo(string userId)
        {
            Customer customer = _customerManager.Customer.GetCustomer(userId, false);
            CustomerForReadInCustomerDto customerDto = _mapper.Map<CustomerForReadInCustomerDto>(customer);
            return customerDto;
        }

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters)
        {
            var customers = _guestManager.Customers.GetCustomers(parameters);
            return customers;
        }
    }
}
