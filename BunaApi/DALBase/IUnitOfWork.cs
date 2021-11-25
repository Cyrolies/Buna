

namespace DALBase
{
    /// <summary>
    /// The Unit of Work base contract
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the orm.
        /// </summary>
        /// <value>The orm.</value>
        object Orm { get; }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        int Add<T>(T entity) where T : class;

        /// <summary>
        /// Adds the entity return entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T AddEntityReturnEntity<T>(T entity) where T : class;

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        int Update<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        int Delete<T>(T entity) where T : class;

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        void RollbackTransaction();
    }
}


