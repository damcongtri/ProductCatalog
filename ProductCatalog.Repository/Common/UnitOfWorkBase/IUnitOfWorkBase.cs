using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Common.UnitOfWorkBase
{
    public interface IUnitOfWorkBase : IDisposable
    {
        Task<int> CommitAsync();

    }
}
