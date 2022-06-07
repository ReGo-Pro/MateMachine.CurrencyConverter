using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface IRepository<TEntity> where TEntity : class {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
    }
}
