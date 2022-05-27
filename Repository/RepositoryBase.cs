﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext _repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task CreateAsync(T entity) => await _repositoryContext.AddAsync(entity);
        
        public void Update(T entity) => _repositoryContext.Update(entity);

        public void Delete(T entity) => _repositoryContext.Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            _repositoryContext.Set<T>().AsNoTracking() :
            _repositoryContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
            _repositoryContext.Set<T>().Where(expression).AsNoTracking() :
            _repositoryContext.Set<T>().Where(expression);
    }
}
