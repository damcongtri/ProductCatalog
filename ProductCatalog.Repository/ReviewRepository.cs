using ProductCatalog.Model.Database;
using ProductCatalog.Repository.Common.BaseRepository;
using ProductCatalog.Repository.Common.DbContext;
using ProductCatalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IDbContext context) : base(context)
        {
        }
    }
}
