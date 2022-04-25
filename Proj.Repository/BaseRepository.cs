using Microsoft.EntityFrameworkCore;
using Proj.Core.Domains;
using Proj.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Base
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public IEnumerable<T> GetWithFilter(Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>().Where(filter).ToList();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> option)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(option);
        }

        public async Task<T> SingleOrDefault(Expression<Func<T, bool>> option)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(option);
        }

        public async Task AddAsync(T item)
        {
            await _context.AddAsync(item);
            this.SaveChanges();
        }

        public void Update(T item)
        {
            _context.Update(item);
            this.SaveChanges();
        }

        public void Delete(T item)
        {
            _context.Remove(item);
            this.SaveChanges();
        }

        public bool Any(Expression<Func<T, bool>> option)
        {
            return _context.Set<T>().Any(option);
        }

        public bool All(Expression<Func<T, bool>> option)
        {
            return _context.Set<T>().All(option);
        }

        public int Count()
        {
            return this._context.Set<T>().Count();
        }
        public int Count(Expression<Func<T,bool>> option)
        {
            return this._context.Set<T>().Count(option);
        }
        
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
