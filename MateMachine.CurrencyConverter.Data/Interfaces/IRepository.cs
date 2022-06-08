using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface IRepository<TEntity> where TEntity : class {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

        TEntity Find(int id);
        IEnumerable<TEntity> GetAll();
    }
}
