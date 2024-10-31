using ProductCatalog.Repository.Common.UnitOfWorkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Interfaces
{
    public interface IUnitOfWork : IUnitOfWorkBase
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }

        ICartRepository CartRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        ICouponRepository CouponRepository { get; }
        IReviewRepository ReviewRepository { get; }

    }
}
