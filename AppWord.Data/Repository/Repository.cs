using AppWord.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppWordDbContext Context;
        protected readonly DbSet<T> Entities;

        public Repository(AppWordDbContext context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public virtual T Add(T t)
        {
            Entities.Add(t);
            SaveChanges();
            return t;
        }

        public virtual async Task<T> AddAsync(T t)
        {
            await Entities.AddAsync(t);
            await SaveChangesAsync();
            return t;
        }

        public void ChangeState(T t, EntityState entityState)
        {
            Context.Entry(t).State = entityState;
        }
        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> t)
        {
            await Entities.AddRangeAsync(t);
            await SaveChangesAsync();
            return t;
        }
        public int Count()
        {
            return Entities.Count();
        }

        public async Task<int> CountAsync()
        {
            return await Entities.CountAsync();
        }

        public virtual void Delete(T entity)
        {
            DeleteAsync(entity).Wait();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            await SaveChangesAsync();
        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return Entities.SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return Entities.Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await Entities.Where(match).ToListAsync();
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await Entities.FirstOrDefaultAsync(match);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = Entities.Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public virtual T Get(int id)
        {
            return id == 0 ? null : Entities.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return Entities.AsQueryable();
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            var queryable = GetAll();
            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public virtual async Task<T> GetAsync(int id)
        {
            if (id == 0)
                return null;

            return await Entities.FindAsync(id);
        }

        public bool IsExists(int id)
        {
            return Entities.Any(e => e.Id == id);
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;
            var exist = Entities.Find(key);
            if (exist != null)
            {
                Context.Entry(exist).CurrentValues.SetValues(t);
                Context.Entry(exist).State = EntityState.Modified;
                SaveChanges();
            }

            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, object key)
        {
            if (t == null)
                return null;
            var exist = await Entities.FindAsync(key);
            if (exist == null) return null;
            Context.Entry(exist).CurrentValues.SetValues(t);
            Context.Entry(exist).State = EntityState.Modified;
            await SaveChangesAsync();
            return exist;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            var modifiedEntries = Context.ChangeTracker.Entries()
                .Where(x => x.Entity is AuditableEntity
                && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is not AuditableEntity) continue;
                var entity = entry.Entity as AuditableEntity;
                if (entity == null) continue;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreateDate = entity.CreateDate > DateTime.MinValue
                            ? entity.CreateDate
                            : DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entity.IsDeleted = true;
                        Context.Entry(entity).State = EntityState.Modified;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Modified:
                        break;
                    default:
                        Context.Entry(entity).Property(x => x.CreateDate).IsModified = false;
                        break;
                }

                entity.ModifiedDate = entity.ModifiedDate > DateTime.MinValue
                    ? entity.ModifiedDate
                    : DateTime.UtcNow;
            }

            return await Context.SaveChangesAsync();
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
