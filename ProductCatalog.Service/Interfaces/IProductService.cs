using Azure;
using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        Task<bool> DeleteProductAsync(int productId);
        Task<List<ProductDto>> GetProductsAsync(ProductQueryParamsDto queryParams);
        Task<List<ProductDto>> GetProductsForShopAsync(int shopId, int page = 1, int take = 10);
        Task<bool> UpdateProduct(UpdateProductDto productDto);
        Task<ProductDto> GetProductByIdAsync(int Id);
        Task<ProductDto> AddProductWithVariantsAsync(AddProductDto productDto);
        Task<VariantAttributeDto> AddVariantAttributeAsync(AddAttributeDto variantAttribute);
        Task<VariantAttributeValue> AddVariantAttributeValueAsync(AddAttributeValueDto variantAttributeValue);
        Task<bool> DeleteProductVariantAsync(int productVariantId);
        Task<List<VariantAttribute>> FindVariantAttributesByProductIdAsync(int productId);
        Task<List<VariantAttribute>> FindVariantAttributesAsync(string? keywork, int? id = null);
        Task<List<VariantAttributeValue>> FindVariantAttributeValuesAsync(Expression<Func<VariantAttributeValue, bool>> predicate);
        Task<ProductVariant> UpdateProductVariantAsync(ProductVariantDto productVariant, params string[] properties);
        Task<VariantAttribute> UpsertVariantAttributeAsync(VariantAttribute variantAttribute);
        Task<VariantAttributeValue> UpsertVariantAttributeValueAsync(VariantAttributeValue variantAttributeValue);
        Task<List<VariantAttribute>> GetProductAttributesAsync(int productId);
        Task<List<VariantAttribute>> GetVariantAttributes(string? keywork, int? id);
        
    }
}
