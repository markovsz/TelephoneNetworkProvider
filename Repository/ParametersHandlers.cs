using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal static class ParametersHandlers
    {
        public static IQueryable<Call> CallParametersHandler(this IQueryable<Call> calls, CallParameters parameters)
        {
            if (parameters.PhoneNumberPart is not null)
                calls = calls
                    .Include(c => c.Caller)
                    .Include(c => c.CalledBy)
                    .Where(c => c.Caller.PhoneNumber.Contains(parameters.PhoneNumberPart) || c.CalledBy.PhoneNumber.Contains(parameters.PhoneNumberPart));

            if (parameters.MinCallInitiationTime.HasValue)
            {
                DateTime minCallInitiationTime = parameters.MinCallInitiationTime.Value;
                calls = calls
                    .Where(c => minCallInitiationTime.CompareTo(c.CallInitiationTime) <= 0);
            }

            if (parameters.MaxCallInitiationTime.HasValue)
            {
                DateTime maxCallInitiationTime = parameters.MaxCallInitiationTime.Value;
                calls = calls.Where(c => maxCallInitiationTime.CompareTo(c.CallInitiationTime) >= 0);
            }

            if (parameters.MinCallEndTime.HasValue)
            {
                DateTime minCallEndTime = parameters.MinCallEndTime.Value;
                calls = calls.Where(c => minCallEndTime.CompareTo(c.CallEndTime) <= 0);
            }

            if (parameters.MinCallEndTime.HasValue)
            {
                DateTime maxCallEndTime = parameters.MaxCallEndTime.Value;
                calls = calls.Where(c => maxCallEndTime.CompareTo(c.CallEndTime) >= 0);
            }

            return calls
                .RequestParametersHandler(parameters);
        }

        public static IQueryable<Customer> CustomerParametersHandler(this IQueryable<Customer> customers, CustomerParameters parameters)
        {
            //TODO

            return customers
                .RequestParametersHandler(parameters);
        }

        public static IQueryable<AdministratorMessage> AdministratorMessageParametersHandler(this IQueryable<AdministratorMessage> messages, AdministratorMessageParameters parameters)
        {
            //TODO

            return messages
                .RequestParametersHandler(parameters);
        }

        private static IQueryable<T> RequestParametersHandler<T>(this IQueryable<T> elements, RequestParameters parameters)
        {
            int pageSize = RequestParameters.MaxPageSize;
            if (parameters.PageSize.HasValue)
            {
                if (parameters.PageSize.Value > 0 &&
                parameters.PageSize.Value <= RequestParameters.MaxPageSize)
                {
                    pageSize = parameters.PageSize.Value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            int customersCount = elements.Count();
            if (parameters.PageNumber < 1 || parameters.PageNumber > (customersCount + pageSize - 1) / pageSize)
            {
                throw new ArgumentOutOfRangeException();//PageNumberOutOfRangeException();
            }

            return elements
                .Skip((parameters.PageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
