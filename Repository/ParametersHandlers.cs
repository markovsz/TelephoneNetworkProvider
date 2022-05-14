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
                    .Where(c => c.Caller.PhoneNumber.Contains(parameters.PhoneNumberPart) 
                            || c.CalledBy.PhoneNumber.Contains(parameters.PhoneNumberPart));

            if (parameters.MinCallInitiationTime.HasValue)
                calls = calls
                    .Where(c => parameters.MinCallInitiationTime.Value
                        .CompareTo(c.CallInitiationTime) <= 0);

            if (parameters.MaxCallInitiationTime.HasValue)
                calls = calls
                    .Where(c => parameters.MaxCallInitiationTime.Value
                        .CompareTo(c.CallInitiationTime) >= 0);

            if (parameters.MinCallEndTime.HasValue)
                calls = calls
                    .Where(c => parameters.MinCallEndTime.Value
                        .CompareTo(c.CallEndTime) <= 0);

            if (parameters.MinCallEndTime.HasValue)
                calls = calls
                    .Where(c => parameters.MaxCallEndTime.Value
                        .CompareTo(c.CallEndTime) >= 0);

            return calls
                .RequestParametersHandler(parameters);
        }

        public static IQueryable<Customer> CustomerParametersHandler(this IQueryable<Customer> customers, CustomerParameters parameters)
        {
            //TODO

            if (parameters.NamePart is not null)
                customers = customers
                    .Where(c => c.Name.Contains(parameters.NamePart));

            if (parameters.SurnamePart is not null)
                customers = customers
                    .Where(c => c.Surname.Contains(parameters.SurnamePart));

            if (parameters.PatronymicPart is not null)
                customers = customers
                    .Where(c => c.Patronymic.Contains(parameters.PatronymicPart));

            if (parameters.PhoneNumberPart is not null)
                customers = customers
                    .Where(c => c.PhoneNumber.Contains(parameters.PhoneNumberPart));

            return customers
                .RequestParametersHandler(parameters);
        }

        public static IQueryable<AdministratorMessage> AdministratorMessageParametersHandler(this IQueryable<AdministratorMessage> messages, AdministratorMessageParameters parameters)
        {
            //TODO
            if (parameters.customerId.HasValue)
                messages = messages
                    .Where(m => m.CustomerId.Equals(parameters.customerId.Value));

            if (parameters.Status is not null)
                messages = messages
                    .Where(m => m.Status.Equals(parameters.Status));

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
            int customersCountOffset = (customersCount == 0 ? 1 : 0);
            if (parameters.PageNumber < 1 || parameters.PageNumber > (customersCount + pageSize - 1 + customersCountOffset) / pageSize)
            {
                throw new ArgumentOutOfRangeException();//PageNumberOutOfRangeException();
            }

            return elements
                .Skip((parameters.PageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
