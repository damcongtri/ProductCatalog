using ProductCatalog.Repository.Common.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Common.UnitOfWorkBase
{
    public abstract class UnitOfWorkBase : IUnitOfWorkBase
    {
        /// <summary>
        /// true means dbContext was disposed
        /// </summary>
        protected bool Disposed;

        /// <summary>
        /// The DbContext
        /// </summary>
        protected readonly IDbContext DbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class.
        /// </summary>
        /// <param name="context">object context</param>
        protected UnitOfWorkBase(IDbContext context)
        {
            DbContext = context;
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public async Task<int> CommitAsync()
        {
            // Save changes with the default options
            return await Task.FromResult(DbContext.SaveChanges());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            DbContext.Dispose();
            Disposed = true;

            if (!disposing)
            {
                return;
            }
        }
    }
}
