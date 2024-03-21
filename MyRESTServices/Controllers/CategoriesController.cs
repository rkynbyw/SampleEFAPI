using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryBLL _categoryBLL;
        private readonly IValidator<CategoryCreateDTO> _validatorCategoryCreate;
        private readonly IValidator<CategoryUpdateDTO> _validatorCategoryUpdate;
        public CategoriesController(ICategoryBLL categoryBLL,
            IValidator<CategoryCreateDTO> validatorCategoryCreate,
            IValidator<CategoryUpdateDTO> validatorCategoryUpdate)
        {
            _categoryBLL = categoryBLL;
            _validatorCategoryCreate = validatorCategoryCreate;
            _validatorCategoryUpdate = validatorCategoryUpdate;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var results = await _categoryBLL.GetAll();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var result = await _categoryBLL.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryCreateDTO categoryCreateDTO)
        {
            if (categoryCreateDTO == null)
            {
                return BadRequest();
            }

            var validatorResult = await _validatorCategoryCreate.ValidateAsync(categoryCreateDTO);
            if (!validatorResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _categoryBLL.Insert(categoryCreateDTO);
                return Ok("Insert data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            if (id != categoryUpdateDTO.CategoryId)
            {
                return BadRequest();
            }

            var validatorResult = await _validatorCategoryUpdate.ValidateAsync(categoryUpdateDTO);
            if (!validatorResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _categoryBLL.Update(categoryUpdateDTO);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok("Update data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryBLL.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                await _categoryBLL.Delete(id);
                return Ok("Delete data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCountCategories(string name)
        {
            try
            {
                var count = await _categoryBLL.GetCountCategories(name);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetWithPaging(int pageNumber, int pageSize, string name = "")
        {
            try
            {
                var categories = await _categoryBLL.GetWithPaging(pageNumber, pageSize, name);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
