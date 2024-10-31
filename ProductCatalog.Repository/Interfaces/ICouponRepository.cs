using ProductCatalog.Model.Database;
using ProductCatalog.Repository.Common.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Interfaces
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
    }
}
