using ProductCatalog.Model.Dto.CategoryDtos;
using ProductCatalog.Model.Database;
using AutoMapper;
using ProductCatalog.Model.Dto.ProductDtos;
using ProductCatalog.Model.Dto.CartDtos;
namespace ProductCatalog.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping từ Category -> CategoryDTO
            CreateMap<Category, CategoryDTO>();
                //.ForMember(dest => dest.Childrend, opt => opt.MapFrom(src => src.ToList());

            // Mapping từ CreateCategoryDTO -> Category
            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping từ UpdateCategoryDTO -> Category
            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.ProductVariants));
            
            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(dest=> dest.AttributesValue, opt => opt.MapFrom(src => src.ProductVariantAttributeLinks
                .Select(x =>new AttibuteValueDto {
                    ValueId = x.ValueId,
                    Value = x.VariantAttributeValue.Value,
                    Attribute = x.VariantAttributeValue.VariantAttribute.Name,
                    CreatedAt = x.VariantAttributeValue.CreatedAt
                }).ToList()));

            CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.ProductVariants, opt => opt.MapFrom(src =>
            src.Variants.Select(variantDto => new ProductVariant
            {
                VariantSku = variantDto.VariantSku,
                Name = variantDto.Name,
                Price = variantDto.Price,
                StockQuantity = variantDto.StockQuantity,
                ImageUrl = variantDto.ImageUrl,
                ProductVariantAttributeLinks = variantDto.AttributesValue.Select(x => new ProductVariantAttributeLink
                {
                    ValueId = x.ValueId, // Cần đảm bảo rằng bạn đã có các thuộc tính này
                }).ToList()
            }).ToList()));

            CreateMap<ProductVariantDto, ProductVariant>()
            .ForMember(dest => dest.ProductVariantAttributeLinks, opt => opt.MapFrom(src =>
            src.AttributesValue.Select(av => new ProductVariantAttributeLink
            {
                ValueId = av.ValueId // Cần đảm bảo rằng bạn đã có các thuộc tính này
            }).ToList()));

            CreateMap<VariantAttributeDto, VariantAttribute>();
            CreateMap<VariantAttribute, VariantAttributeDto>();

            CreateMap<AddToCartDto, Cart>();
            CreateMap<Cart, CartItemDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<UpdateCategoryDTO, Cart>();

            CreateMap<Category, CategoryWithParentDto>();



        }
    }
}
