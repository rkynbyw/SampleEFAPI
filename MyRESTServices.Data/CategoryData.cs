using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data
{
    public class CategoryData : ICategoryData
    {
        private readonly AppDbContext _context;
        public CategoryData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetByName(string name)
        {
            return await _context.Categories.Where(c => c.CategoryName.Contains(name)).ToListAsync();
        }

        public async Task<int> GetCountCategories(string name)
        {
            return await _context.Categories.Where(c => c.CategoryName.Contains(name)).CountAsync();
        }

        public async Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string name = null)
        {
            IQueryable<Category> query = _context.Categories.OrderBy(c => c.CategoryName);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.CategoryName.Contains(name));
            }

            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }


        public async Task<Category> Insert(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> InsertWithIdentity(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task<Category> Update(int id, Category entity)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
                return null;

            existingCategory.CategoryName = entity.CategoryName;
            await _context.SaveChangesAsync();
            return existingCategory;
        }
    }
}
