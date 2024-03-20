using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data
{
    public class ArticleData : IArticleData
    {
        private readonly AppDbContext _context;
        public ArticleData(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticleByCategory(int categoryId)
        {
            return await _context.Articles.Where(a => a.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticleWithCategory()
        {
            return await _context.Articles.Include(a => a.Category).ToListAsync();
        }

        public async Task<Article> GetById(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<int> GetCountArticles()
        {
            return await _context.Articles.CountAsync();
        }

        public async Task<IEnumerable<Article>> GetWithPaging(int categoryId, int pageNumber, int pageSize)
        {
            return await _context.Articles.Where(a => a.CategoryId == categoryId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Article> Insert(Article entity)
        {
            _context.Articles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Article> InsertArticleWithCategory(Article article)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Articles.Add(article);
                    await _context.SaveChangesAsync();

                    var category = new Category { CategoryName = article.Category.CategoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();

                    article.CategoryId = category.CategoryId;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return article;
        }



        public async Task<int> InsertWithIdentity(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article.ArticleId;
        }

        public async Task<Article> Update(int id, Article entity)
        {
            var existingArticle = await _context.Articles.FindAsync(id);
            if (existingArticle == null)
                return null;

            existingArticle.CategoryId = entity.CategoryId;
            existingArticle.Title = entity.Title;
            existingArticle.Details = entity.Details;
            existingArticle.PublishDate = entity.PublishDate;
            existingArticle.IsApproved = entity.IsApproved;
            existingArticle.Pic = entity.Pic;

            await _context.SaveChangesAsync();

            return existingArticle;
        }
    }
}
