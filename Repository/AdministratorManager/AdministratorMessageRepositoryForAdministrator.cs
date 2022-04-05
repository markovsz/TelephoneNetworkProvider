﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository
{
    public class AdministratorMessageRepositoryForAdministrator : RepositoryBase<AdministratorMessage>, IAdministratorMessageRepositoryForAdministrator
    {

        public AdministratorMessageRepositoryForAdministrator(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<AdministratorMessage> GetMessage(uint id) =>
            FindByCondition(m => m.Id.Equals(id), false);

        public IEnumerable<AdministratorMessage> GetCustomerMessages(string userId, AdministratorMessageParameters parameters) =>
            FindByCondition(m => m.UserId.Equals(userId), false)
            .AdministratorMessageParametersHandler(parameters)
            .ToList();

        public IEnumerable<AdministratorMessage> GetCustomerWarningMessagesFromTime(string userId, DateTime startTime) =>
            FindByCondition(m => m.UserId.Equals(userId), false)
            .Where(m => m.Status.Equals("warning"))
            .Where(m => m.SendingTime.CompareTo(startTime) == 1)
            .ToList();

        public void CreateMessage(AdministratorMessage message) => Create(message);

        public void DeleteMessage(uint id) =>
            Delete(FindByCondition(m => m.Id.Equals(id), true).FirstOrDefault());
    }
}
