using Microsoft.EntityFrameworkCore;
using ProductCatalog.Model.Database;
using ProductCatalog.Repository.Common.BaseRepository;
using ProductCatalog.Repository.Common.DbContext;
using ProductCatalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        public ProductRepository(IDbContext context) : base(context)
        {
        }

        public async Task<Product> AddProductWithVariantAsync(Product product)
        {
            // Thêm sản phẩm và các biến thể của nó vào DbContext
            await DbSet.AddAsync(product);

            // Không gọi SaveChangesAsync() ở đây, việc này sẽ được quản lý bởi UnitOfWork

            return product;
        }

        public async Task<VariantAttribute> AddVariantAttributeAsync(VariantAttribute variantAttribute)
        {
            await Entities.Set<VariantAttribute>().AddAsync(variantAttribute);


            return variantAttribute;
        }

        public async Task<VariantAttributeValue> AddVariantAttributeValueAsync(VariantAttributeValue variantAttributeValue)
        {
            await Entities.Set<VariantAttributeValue>().AddAsync(variantAttributeValue);


            return variantAttributeValue;
        }

        public ProductVariant DeleteProductVariant(ProductVariant productVariant)
        {
            return Entities.Set<ProductVariant>().Remove(productVariant).Entity;
        }

        public async Task<List<VariantAttribute>> FindVariantAttributeAsync(Expression<Func<VariantAttribute, bool>> predicate)
        {
            return await Entities.Set<VariantAttribute>()
                                 .Where(predicate)
                                 .ToListAsync();
        }

        public async Task<List<VariantAttributeValue>> FindVariantAttributeValueAsync(Expression<Func<VariantAttributeValue, bool>> predicate)
        {
            return await Entities.Set<VariantAttributeValue>()
                                 .Where(predicate)
                                 .ToListAsync();
        }

        public IQueryable<VariantAttribute> GetVariantAttribute()
        {
            return Entities.Set<VariantAttribute>();
        }

        public IQueryable<VariantAttributeValue> GetVariantAttributeValue()
        {
            return Entities.Set<VariantAttributeValue>();
        }

        public  ProductVariant UpdateProductVariantAsync(ProductVariant productVariant, params string[] properties)
            {
                // Gắn entity vào DbContext mà không đánh dấu nó là đã thay đổi
                Entities.Set<ProductVariant>().Attach(productVariant);

                // Nếu không có thuộc tính nào được chỉ định, đánh dấu toàn bộ entity là Modified
                if (properties.Length == 0)
                {
                    Entities.Entry(productVariant).State = EntityState.Modified;
                }
                else
                {
                    // Nếu có các thuộc tính được chỉ định, chỉ đánh dấu chúng là Modified
                    foreach (var property in properties)
                    {
                        Entities.Entry(productVariant).Property(property).IsModified = true;
                    }
                }

                return productVariant;
            }


        public async Task<VariantAttribute> UpsertVariantAttributeAsync(VariantAttribute variantAttribute)
        {
            var existing = await Entities.Set<VariantAttribute>()
                                         .FirstOrDefaultAsync(a => a.AttributeId == variantAttribute.AttributeId);

            if (existing == null)
            {
                await Entities.Set<VariantAttribute>().AddAsync(variantAttribute);
            }
            else
            {
                Entities.Entry(existing).CurrentValues.SetValues(variantAttribute);
            }


            return variantAttribute;
        }

        public async Task<VariantAttributeValue> UpsertVariantAttributeValueAsync(VariantAttributeValue variantAttributeValue)
        {
            var existing = await Entities.Set<VariantAttributeValue>()
                                         .FirstOrDefaultAsync(v => v.ValueId == variantAttributeValue.ValueId);

            if (existing == null)
            {
                await Entities.Set<VariantAttributeValue>().AddAsync(variantAttributeValue);
            }
            else
            {
                Entities.Entry(existing).CurrentValues.SetValues(variantAttributeValue);
            }


            return variantAttributeValue;
        }
    }


}
