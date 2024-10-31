using ProductCatalog.Model.Database;
using ProductCatalog.Repository.Common.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Thêm sản phẩm cùng với các biến thể của nó.
        Task<Product> AddProductWithVariantAsync(Product product);

        // Thêm mới một thuộc tính của biến thể.
        Task<VariantAttribute> AddVariantAttributeAsync(VariantAttribute variantAttribute);

        // Thêm mới một giá trị thuộc tính của biến thể.
        Task<VariantAttributeValue> AddVariantAttributeValueAsync(VariantAttributeValue variantAttributeValue);

        // Xóa một biến thể sản phẩm.
        ProductVariant DeleteProductVariant(ProductVariant productVariant);

        // Tìm kiếm danh sách các thuộc tính biến thể theo điều kiện.
        Task<List<VariantAttribute>> FindVariantAttributeAsync(Expression<Func<VariantAttribute, bool>> predicate);

        // Tìm kiếm danh sách các giá trị thuộc tính biến thể theo điều kiện.
        Task<List<VariantAttributeValue>> FindVariantAttributeValueAsync(Expression<Func<VariantAttributeValue, bool>> predicate);

        // Cập nhật một biến thể sản phẩm với tùy chọn chỉ cập nhật các thuộc tính cụ thể.
        ProductVariant UpdateProductVariantAsync(ProductVariant productVariant, params string[] properties);

        // Thêm mới hoặc cập nhật một thuộc tính biến thể (upsert).
        Task<VariantAttribute> UpsertVariantAttributeAsync(VariantAttribute variantAttribute);

        // Thêm mới hoặc cập nhật một giá trị thuộc tính biến thể (upsert).
        Task<VariantAttributeValue> UpsertVariantAttributeValueAsync(VariantAttributeValue variantAttributeValue);

        IQueryable<VariantAttribute> GetVariantAttribute();
        IQueryable<VariantAttributeValue> GetVariantAttributeValue();

    }

}
