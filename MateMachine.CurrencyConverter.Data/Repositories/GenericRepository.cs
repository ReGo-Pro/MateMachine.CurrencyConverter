using MateMachine.CurrencyConverter.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MateMachine.CurrencyConverter.Data.Repositories {
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class {
        protected DbContext dbContext;

        public GenericRepository(DbContext dbContext) {
            this.dbContext = dbContext;
        }

        public void Add(TEntity entity) {
            dbContext.Add(entity);
        }

        public void Remove(TEntity entity) {
            dbContext.Remove(entity);
        }

        public TEntity Find(int id) {
            return (TEntity)dbContext.Find(typeof(TEntity), id);
        }

        public virtual IEnumerable<TEntity> GetAll() {
            return dbContext.Set<TEntity>().ToList();
        }

        public void Update(TEntity entity) {
            dbContext.Update(entity);
        }
    }
}
