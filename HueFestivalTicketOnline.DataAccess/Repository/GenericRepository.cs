﻿using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
             dbSet.Remove(entity);
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            List<Expression<Func<T, bool>>> includes = null ,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includesProperties = null,
            int skip =0,
            int take =0)
        {
            IQueryable<T> query = dbSet;
            if(filter !=null)
            {
                query = query.Where(filter);
            }
            if(includesProperties != null)
            {
                foreach(string property in includesProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            if(skip > 0)
            {
                query = query.Skip(skip);
            }
            if(take > 0)
            {
                query = query.Take(take);
            }
            if(includes != null)
            {
                foreach(Expression<Func<T,bool>> include in includes)
                {
                    query = query.Where(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includesProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includesProperties != null)
            {
                foreach (string property in includesProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }
    }
}