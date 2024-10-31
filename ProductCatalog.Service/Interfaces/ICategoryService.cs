using ProductCatalog.Model.Dto.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service.BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO);
        Task<bool> DeleteCategoryAsync(int id);

        Task<List<CategoryDTO>> GetCategoryChildrenById(int id);
    }
}
