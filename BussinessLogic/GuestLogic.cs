using System;
using System.Collections.Generic;
using Entities.Models;
using Entities.RequestFeatures;
using Repository;

namespace BussinessLogic
{
    public class GuestLogic : IGuestLogic
    {
        private IGuestManager _guestManager;

        public GuestLogic(IGuestManager guestManager)
        {
            _guestManager = guestManager;
        }

        public IEnumerable<Customer> GetCustomers(CustomerParameters parameters)
        {
            return _guestManager.Customers.GetCustomers(parameters);
        }
    }
}