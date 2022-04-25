using Proj.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Repositories
{
    public interface IBaseRepository<T> where T : Base
    {
        public Task<IEnumerable<T>> GetAll();
        public IEnumerable<T> GetWithFilter(Expression<Func<T, bool>> filter);
        public Task<T> FindByIdAsync(int id);
        public Task AddAsync(T item);
        public bool Any(Expression<Func<T, bool>> filter);
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> option);
        public Task<T> SingleOrDefault(Expression<Func<T, bool>> option);
        public bool All(Expression<Func<T, bool>> filter);
        public void Update(T item);
        public void Delete(T item);

        public int Count();
        public int Count(Expression<Func<T, bool>> option);
        public int SaveChanges();

    }
}
