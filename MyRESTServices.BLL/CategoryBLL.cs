using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.BLL
{
    public class CategoryBLL : ICategoryBLL
    {
        private readonly ICategoryData _categoryData;
        private readonly IMapper _mapper;

        public CategoryBLL(ICategoryData categoryData, IMapper mapper)
        {
            _categoryData = categoryData;
            _mapper = mapper;
        }

        public async Task<bool> Delete(int id)
        {
            return await _categoryData.Delete(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await _categoryData.GetAll();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return categoriesDto;
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var category = await _categoryData.GetById(id);
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return categoryDto;
        }

        public async Task<IEnumerable<CategoryDTO>> GetByName(string name)
        {
            var categories = await _categoryData.GetByName(name);
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return categoriesDto;
        }

        public async Task<int> GetCountCategories(string name)
        {
            return await _categoryData.GetCountCategories(name);
        }

        public async Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            var categories = await _categoryData.GetWithPaging(pageNumber, pageSize, name);
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return categoriesDto;
        }

        public async Task<CategoryDTO> Insert(CategoryCreateDTO entity)
        {
            var category = _mapper.Map<Category>(entity);
            var insertedCategory = await _categoryData.Insert(category);
            return _mapper.Map<CategoryDTO>(insertedCategory);
        }

        public async Task<CategoryDTO> Update(CategoryUpdateDTO entity)
        {
            var category = _mapper.Map<Category>(entity);
            var updatedCategory = await _categoryData.Update(entity.CategoryId, category);
            return _mapper.Map<CategoryDTO>(updatedCategory);
        }


    }
}
