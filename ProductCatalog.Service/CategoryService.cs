using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.CategoryDtos;
using ProductCatalog.Repository.Common.DbContext;
using ProductCatalog.Repository.Interfaces;
using ProductCatalog.Service.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadFile;
namespace ProductCatalog.Service.BusinessLogic
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Lấy tất cả các Category
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categoryDTOs = _unitOfWork.CategoryRepository.GetALl().Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                ParentCategoryId = c.ParentCategoryId,
                MetaTitle = c.MetaTitle,
                MetaDescription = c.MetaDescription,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive
            }).ToList();

            return BuildHierarchy(categoryDTOs, null);
        }

        private List<CategoryDTO> BuildHierarchy(List<CategoryDTO> categories, int? parentId)
        {
            return categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    MetaTitle = c.MetaTitle,
                    MetaDescription = c.MetaDescription,
                    ImageUrl = c.ImageUrl,
                    IsActive = c.IsActive,
                    Children = BuildHierarchy(categories, c.CategoryId)
                })
                .ToList();
        }

        // Lấy Category theo Id
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            return _mapper.Map<CategoryDTO>(category);
        }

        // Tạo mới Category
        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            if (createCategoryDTO.ImageFile != null)
            {
                createCategoryDTO.ImageUrl = await UploadToDrive.UploadFileAsync(createCategoryDTO.ImageFile);
            }
            var category = _mapper.Map<Category>(createCategoryDTO);
            _unitOfWork.CategoryRepository.AddAsync(category);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<CategoryDTO>(category);
        }

        // Cập nhật Category
        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO)
        {
            if (updateCategoryDTO.ImageFile != null)
            {
                updateCategoryDTO.ImageUrl = await UploadToDrive.UploadFileAsync(updateCategoryDTO.ImageFile);
            }
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(updateCategoryDTO.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.CategoryName = updateCategoryDTO.CategoryName;
            category.Description = updateCategoryDTO.Description;
            category.ParentCategoryId = updateCategoryDTO.ParentCategoryId;
            category.MetaTitle = updateCategoryDTO.MetaTitle;
            category.MetaDescription = updateCategoryDTO.MetaDescription;
            category.ImageUrl = updateCategoryDTO.ImageUrl;
            category.IsActive = updateCategoryDTO.IsActive;
            _unitOfWork.CategoryRepository.UpdateAsync(category);

           return await _unitOfWork.CommitAsync() >0;
        }

        // Xóa Category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _unitOfWork.CategoryRepository.Delete(category);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<List<CategoryDTO>> GetCategoryChildrenById(int id)
        {
            // Đảm bảo FindBy trả về IQueryable để có thể gọi Include
            var categories = await _unitOfWork.CategoryRepository
                .FindBy(x => x.ParentCategoryId == id) // Trả về IQueryable
                .Include(x => x.ParentCategory) // Bao gồm ParentCategory
                .ToListAsync(); // Thực hiện truy vấn với ToListAsync

            // Mapping kết quả sang DTO
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

    }

}
