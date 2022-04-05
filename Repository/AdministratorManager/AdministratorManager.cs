﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
;

namespace Repository
{
    public class AdministratorManager : IAdministratorManager
    {
        private RepositoryContext _repositoryContext;
        private ICustomerRepositoryForAdministrator _customerRepository;
        private ICallRepositoryForAdministrator _callRepository;
        private IAdministratorMessageRepositoryForAdministrator _administratorMessageRepository;
        public AdministratorManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICustomerRepositoryForAdministrator Customers
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new CustomerRepositoryForAdministrator(_repositoryContext);
                return _customerRepository;
            }
        }

        public ICallRepositoryForAdministrator Calls
        {
            get
            {
                if (_callRepository == null)
                    _callRepository = new CallRepositoryForAdministrator(_repositoryContext);
                return _callRepository;
            }
        }

        public IAdministratorMessageRepositoryForAdministrator Messages
        {
            get
            {
                if (_administratorMessageRepository == null)
                    _administratorMessageRepository = new AdministratorMessageRepositoryForAdministrator(_repositoryContext);
                return _administratorMessageRepository;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
