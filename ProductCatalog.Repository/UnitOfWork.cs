using ProductCatalog.Repository.Common.DbContext;
using ProductCatalog.Repository.Common.UnitOfWorkBase;
using ProductCatalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ICartRepository _cartRepository;
        private IReviewRepository _reviewRepository;
        private ICouponRepository _couponRepository;
        private IWishlistRepository _wishlistRepository;

        

        public UnitOfWork(IDbContext context) : base(context)
        {
        }
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(DbContext);
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(DbContext);
        public ICartRepository CartRepository => _cartRepository ??= new CartRepository(DbContext);
        public IReviewRepository ReviewRepository => _reviewRepository ??= new ReviewRepository(DbContext);
        public ICouponRepository CouponRepository => _couponRepository ??= new CouponRepository(DbContext);
        public IWishlistRepository WishlistRepository => _wishlistRepository ??= new WishlistRepository(DbContext);

    }
}
