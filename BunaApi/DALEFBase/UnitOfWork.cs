using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
//using System.Data.Objects;
using System.Data;
using System.Transactions;
//using System;
//using System.Data.Entity;
using DALBase;
using System.Data.Entity.Core;

namespace DALEFBase
{
    /// <summary>
    /// This is the unit of work object which controls transactional processing
    /// </summary>
    [Export(typeof(IUnitOfWork))]
    public class UnitOfWork : IUnitOfWork 
    {
        private TransactionScope tx;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="orm">The orm.</param>
        public UnitOfWork(DbContext orm)
        {
            this.Orm = orm;
        }

        #region IUnitOfWork Members

        /// <summary>
        /// Gets the orm.
        /// </summary>
        /// <value>
        /// The orm.
        /// </value>
        public object Orm { get; private set; }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Add<T>(T entity) where T : class
        {
            try
            {
               ((DbContext)Orm).Set<T>().Add(entity);
               return ((DbContext)Orm).SaveChanges();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
                      
        }

        /// <summary>
        /// Adds the specified entity and returns the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public T AddEntityReturnEntity<T>(T entity) where T : class
        {
            try
            {
                ((DbContext)Orm).Set<T>().Add(entity);
                ((DbContext)Orm).SaveChanges();
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Update<T>(T entity) where T : class
        {
                try
                {
                        ((DbContext)Orm).Set<T>().Attach(entity);
                        ((DbContext)Orm).Entry(entity).State = System.Data.Entity.EntityState.Modified;
                        return ((DbContext)Orm).SaveChanges();
                        
                }
                catch (OptimisticConcurrencyException conexp)
                {
                   throw conexp;
                   
                }
                catch (Exception ex)
                {
                   throw ex;
                }
                
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Delete<T>(T entity) where T : class
        {
            try
            {
                        ((DbContext)Orm).Set<T>().Attach(entity);
                        ((DbContext)Orm).Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                        return ((DbContext)Orm).SaveChanges();
            
            }
            catch (Exception ex)
            {
               throw ex;
            }
            
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                tx = new TransactionScope();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (tx != null)
                {
                    ((DbContext)Orm).SaveChanges();
                    tx.Complete();
                   
                }
                
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }
            
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (tx != null)
            {
                tx.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// Entities the name of the set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string EntitySetName<T>()
        {
            return String.Format(@"{0}s", typeof(T).Name);
        }
    }
}