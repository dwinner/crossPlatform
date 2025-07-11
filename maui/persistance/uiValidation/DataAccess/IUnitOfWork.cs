namespace c4_LocalDatabaseConnection.DataAccess;

public interface IUnitOfWork<TEntity> where TEntity : class
{
   IRepository<TEntity> Items { get; }
   Task SaveAsync();
}