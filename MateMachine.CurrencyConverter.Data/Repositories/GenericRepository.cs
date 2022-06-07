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

        public void Delete(TEntity entity) {
            dbContext.Remove(entity);
        }

        public TEntity Get(int id) {
            return (TEntity)dbContext.Find(typeof(TEntity), id);
        }

        public IEnumerable<TEntity> GetAll() {
            return dbContext.Set<TEntity>().ToList();
        }

        public void Update(TEntity entity) {
            dbContext.Update(entity);
        }
    }
}
