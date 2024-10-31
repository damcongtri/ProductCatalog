using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.ProductDtos;
using ProductCatalog.Repository.Common.DbContext;
using ProductCatalog.Repository.Interfaces;
using ProductCatalog.Service.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UploadFile;

namespace ProductCatalog.Service.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IDbContext _context;
        private readonly IMapper _mapper;

        public ProductService( IUnitOfWork unitOfWork, IMapper mapper, IDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;

        }

        public async Task<ProductDto> AddProductWithVariantsAsync(AddProductDto addProductDto)
        {
            using var transaction = await _context.BeginTransactionAsync();
            try
            {
                var product = new Product
                {
                    ShopId = addProductDto.ShopId,
                    Name = addProductDto.Name,
                    Description = addProductDto.Description,
                    Price = addProductDto.Price,
                    CategoryId = addProductDto.CategoryId,
                    ImageUrl = addProductDto.ImageFile != null ? UploadToDrive.UploadFileAsync(addProductDto.ImageFile).Result : addProductDto.ImageUrl,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    BranchName = addProductDto.BranchName,
                    ProductVariants = addProductDto.Variants.Select(v => new ProductVariant
                    {
                        VariantSku = v.VariantSku,
                        Name = v.Name,
                        Price = v.Price,
                        StockQuantity = v.StockQuantity,
                        ImageUrl = v.ImageFile != null ? UploadToDrive.UploadFileAsync(v.ImageFile).Result : v.ImageUrl,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        ProductVariantAttributeLinks = v.ProductVariantAttributeLink.Select(l => new ProductVariantAttributeLink
                        {
                            ValueId = l.ValueId
                        }).ToList()
                    }).ToList()
                };

                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.CommitAsync();

                // Commit transaction nếu mọi thứ đều thành công
                await transaction.CommitAsync();

                return _mapper.Map<ProductDto>(product);
            }
            catch
            {
                // Rollback transaction nếu có lỗi xảy ra
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<VariantAttributeDto> AddVariantAttributeAsync(AddAttributeDto variantAttribute)
        {
            // Kiểm tra xem thuộc tính với tên này đã tồn tại chưa
            var existingAttribute = await _unitOfWork.ProductRepository
                .GetVariantAttribute()
                .FirstOrDefaultAsync(a => a.Name.ToLower() == variantAttribute.Name.ToLower());

            if (existingAttribute != null)
            {
                throw new InvalidOperationException("A variant attribute with this name already exists.");
            }

            var newAtt = new VariantAttribute
            {
                Name = variantAttribute.Name,
            };

            var addedAttribute = await _unitOfWork.ProductRepository.AddVariantAttributeAsync(newAtt);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<VariantAttributeDto>(addedAttribute);
        }


        public async Task<VariantAttributeValue> AddVariantAttributeValueAsync(AddAttributeValueDto variantAttributeValueDto)
        {
            // Kiểm tra xem giá trị này đã tồn tại cho AttributeId tương ứng chưa
            var existingAttributeValue = await _unitOfWork.ProductRepository
                .GetVariantAttributeValue()
                .FirstOrDefaultAsync(v => v.Value.ToLower() == variantAttributeValueDto.Value.ToLower()
                                           && v.AttributeId == variantAttributeValueDto.AttributeId);

            if (existingAttributeValue != null)
            {
                throw new InvalidOperationException("A variant attribute value with this value already exists for the specified attribute.");
            }

            var newAttValue = new VariantAttributeValue
            {
                Value = variantAttributeValueDto.Value,
                AttributeId = variantAttributeValueDto.AttributeId
            };

            var addedAttributeValue = await _unitOfWork.ProductRepository.AddVariantAttributeValueAsync(newAttValue);
            await _unitOfWork.CommitAsync();
            return addedAttributeValue;
        }


        public async Task<bool> DeleteProductVariantAsync(int productVariantId)
        {
            // Tìm ProductVariant để xóa
            var productVariant = await _unitOfWork.ProductRepository.DeleteByIdAsync(productVariantId);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<List<VariantAttribute>> FindVariantAttributesAsync(Expression<Func<VariantAttribute, bool>> predicate)
        {
            return await _unitOfWork.ProductRepository.FindVariantAttributeAsync(predicate);
        }

        public async Task<List<VariantAttributeValue>> FindVariantAttributeValuesAsync(Expression<Func<VariantAttributeValue, bool>> predicate)
        {
            return await _unitOfWork.ProductRepository.FindVariantAttributeValueAsync(predicate);
        }

        public async Task<ProductDto> GetProductByIdAsync(int Id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(Id);
           
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductVariant> UpdateProductVariantAsync(ProductVariantDto productVariantDto, params string[] properties)
        {
            var productVariantEntity = _mapper.Map<ProductVariant>(productVariantDto);
            var updatedVariant = _unitOfWork.ProductRepository.UpdateProductVariantAsync(productVariantEntity, properties);
            await _unitOfWork.CommitAsync();
            return updatedVariant;
        }

        public async Task<VariantAttribute> UpsertVariantAttributeAsync(VariantAttribute variantAttribute)
        {
            var result = await _unitOfWork.ProductRepository.UpsertVariantAttributeAsync(variantAttribute);
            await _unitOfWork.CommitAsync();
            return result;
        }

        public async Task<VariantAttributeValue> UpsertVariantAttributeValueAsync(VariantAttributeValue variantAttributeValue)
        {
            var result = await _unitOfWork.ProductRepository.UpsertVariantAttributeValueAsync(variantAttributeValue);
            await _unitOfWork.CommitAsync();
            return result;
        }

        public async Task<bool> UpdateProduct(UpdateProductDto productDto)
        {
            var product = new Product
            {
                ShopId = productDto.ShopId,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                ImageUrl = productDto.ImageFile != null ? UploadToDrive.UploadFileAsync(productDto.ImageFile).Result : productDto.ImageUrl,
                CreatedAt = DateTime.Now,
                IsActive = true,
                ProductVariants = productDto.Variants.Select(v => new ProductVariant
                {
                    VariantSku = v.VariantSku,
                    Name = v.Name,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity,
                    ImageUrl = v.ImageFile != null ? UploadToDrive.UploadFileAsync(v.ImageFile).Result : v.ImageUrl,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    ProductVariantAttributeLinks = v.ProductVariantAttributeLink.Select(l => new ProductVariantAttributeLink
                    {
                        ValueId = l.ValueId,
                        VariantId = v.VariantId,
                    }).ToList()
                }).ToList()
            };

            // Cập nhật sản phẩm
            var productUpdate =  _unitOfWork.ProductRepository.UpdateAsync(product);

            // Lưu các thay đổi vào cơ sở dữ liệu
            var result = await _unitOfWork.CommitAsync();

            // Trả về true nếu có ít nhất một bản ghi được cập nhật
            return result > 0;
        }

        public Task<List<VariantAttribute>> FindVariantAttributesByProductIdAsync(int productId)
        {
            throw new NotImplementedException();
        }


        public async Task<List<VariantAttribute>> GetProductAttributesAsync(int productId)
        {
            var attributes = await _unitOfWork.ProductRepository.GetVariantAttribute().AsNoTracking()
                .Where(va => va.VariantAttributeValues
                    .Any(vav => vav.ProductVariantAttributeLinks
                        .Any(link => link.ProductVariant.ProductId == productId
                                     && link.ProductVariant.IsActive
                                     && link.ProductVariant.Product.IsActive)))
                .Select(va => new VariantAttribute
                {
                    AttributeId = va.AttributeId,
                    Name = va.Name,
                    CreatedAt = va.CreatedAt,
                    VariantAttributeValues = va.VariantAttributeValues
                        .Where(vav => vav.ProductVariantAttributeLinks
                            .Any(link => link.ProductVariant.ProductId == productId
                                         && link.ProductVariant.IsActive
                                         && link.ProductVariant.Product.IsActive))
                        .Select(vav => new VariantAttributeValue
                        {
                            ValueId = vav.ValueId,
                            AttributeId = vav.AttributeId,
                            Value = vav.Value,
                            CreatedAt = vav.CreatedAt,
                        })
                        .ToList() // Convert the filtered VariantAttributeValues to a list
                })
                .ToListAsync(); // Execute the query and convert the result to a list


            return attributes;
        }

        public async Task<List<VariantAttribute>> GetVariantAttributes(string? keywork, int? id)
        {
            var query = _unitOfWork.ProductRepository.GetVariantAttribute().AsNoTracking();
            if (string.IsNullOrWhiteSpace(keywork))
                query = query.Where(x => x.Name.Contains(keywork));
            if (id != null)
            {
                query = query.Where(x => x.AttributeId == id);
            }
            return await query.Take(10).Order().ToListAsync();

        }

        public async Task<List<ProductDto>> GetProductsAsync(ProductQueryParamsDto queryParams)
        {
            // Start the query with the list of products from the repository.
            var query = _unitOfWork.ProductRepository.GetALl();

            //// Apply shop ID filter if present.
            //if (queryParams.ShopId.HasValue)
            //{
            //    query = query.Where(p => p.ShopId == queryParams.ShopId.Value);
            //}

            // Apply keyword filter (searching in the name or description).
            //if (!string.IsNullOrWhiteSpace(queryParams.Keyword))
            //{
            //    query = query.Where(p => p.Name.Contains(queryParams.Keyword) || p.Description.Contains(queryParams.Keyword));
            //}

            //// Apply ID filter if present.
            //if (queryParams.Id.HasValue)
            //{
            //    query = query.Where(p => p.ProductId == queryParams.Id.Value);
            //}

            //// Apply category filter if present.
            //if (queryParams.CategoryId.HasValue)
            //{
            //    query = query.Where(p => p.CategoryId == queryParams.CategoryId.Value);
            //}

            //// Apply maximum price filter if present.
            //if (queryParams.MaxPrice.HasValue)
            //{
            //    query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);
            //}

            //// Apply minimum price filter if present.
            //if (queryParams.MinPrice.HasValue)
            //{
            //    query = query.Where(p => p.Price >= queryParams.MinPrice.Value);
            //}

            ////// Apply brand name filter if present.
            ////if (!string.IsNullOrWhiteSpace(queryParams.BranchName))
            ////{
            ////    query = query.Where(p => p.BranchName.Contains(queryParams.BranchName));
            ////}

            //// Apply pagination if both page and pageSize are provided and valid.
            //if (queryParams.Page.HasValue  && queryParams.Page > 0 && queryParams.PageSize > 0)
            //{
            //    int skip = (queryParams.Page.Value - 1) * queryParams.PageSize;
            //    query = query.Skip(skip).Take(queryParams.PageSize);
            //}
            //else if (queryParams.PageSize > 0)
            //{
            //    // Use 'Take' parameter as a fallback to limit the number of products returned.
            //    query = query.Take(queryParams.PageSize);
            //}

            // Execute the query and map the result to ProductDto.
            var products = await query.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }


        public async Task<List<VariantAttribute>> FindVariantAttributesAsync(string? keyword, int? id = null)
        {
            var query = _unitOfWork.ProductRepository.GetVariantAttribute();

            // Lọc theo từ khóa nếu có
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword));
            }

            // Lọc theo id nếu có
            if (id.HasValue)
            {
                query = query.Where(p => p.AttributeId == id);
            }

            // Thực hiện chuyển đổi và ngắt các vòng tham chiếu
            var result = await query.Select(x => new VariantAttribute
            {
                AttributeId = x.AttributeId,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                VariantAttributeValues = x.VariantAttributeValues.Select(v => new VariantAttributeValue
                {
                    ValueId = v.ValueId,
                    Value = v.Value,
                    CreatedAt = v.CreatedAt,

                    // Ngắt vòng lặp bằng cách không đưa vào các tham chiếu ngược
                    ProductVariantAttributeLinks = null,
                    VariantAttribute = null

                }).ToList()
            })
            .Take(10)
            .ToListAsync();

            return result;
        }

        public async Task<List<ProductDto>> GetProductsForShopAsync(int shopId, int page = 1, int take = 10)
        {
            var products = await _unitOfWork.ProductRepository.GetALl().Where(x => x.ShopId == shopId).Skip(page * take).Take(take).ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
           await _unitOfWork.ProductRepository.DeleteByIdAsync(productId);
            return await _unitOfWork.CommitAsync()>0;
        }
    }
}
