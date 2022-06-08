using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface IRepository<TEntity> where TEntity : class {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        TEntity Find(int id);
        IEnumerable<TEntity> GetAll();
    }
}
