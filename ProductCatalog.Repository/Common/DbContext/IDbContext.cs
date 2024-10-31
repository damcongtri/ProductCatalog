using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProductCatalog.Repository.Common.DbContext
{
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Dataset interface
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <returns>Data Set</returns>
        DbSet<T> Set<T>()
            where T : class;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>Number records is effected</returns>
        int SaveChanges();

        /// <summary>
        /// Get entity entry
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Entity Entry Instance </returns>
        EntityEntry Entry(object o);

        /// <summary>
        /// Mark object and property Name as modified
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="propertyName">Property name</param>
        void MarkAsModified(object o, string propertyName);

        Task<T> ExecuteStoredProcedure<T>(string storedProcedure, params SqlParameter[] parameters);
        /// <summary>
        /// Begin a new transaction
        /// </summary>
        /// <param name="isolationLevel">Optional isolation level</param>
        /// <returns>IDbContextTransaction instance</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
