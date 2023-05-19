using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            List<Expression<Func<T, bool>>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includesProperties = null,
            int skip = 0,
            int take =0
            );
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includesProperties = null
            );
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        bool Delete(int id);
        void Update(T entity);


    }
}
