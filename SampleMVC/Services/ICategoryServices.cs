using MyWebFormApp.BLL.DTOs;
using SampleMVC.Models;

namespace SampleMVC.Services
{
	public interface ICategoryServices
	{
		Task<IEnumerable<CategoryDTO>> GetAll();
		Task<Category> GetById(int id);
		Task<CategoryDTO> Insert(CategoryCreateDTO categoryCreateDTO);
		Task Update(int id, CategoryUpdateDTO categoryUpdateDTO);
		Task Delete(int id);

		Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string search);

		Task<int> GetCountCategories(string search);
	}
}
