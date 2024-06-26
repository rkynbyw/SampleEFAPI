﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleBLL _articleBLL;
        private readonly IValidator<ArticleCreateDTO> _validatorArticleCreate;
        private readonly IValidator<ArticleUpdateDTO> _validatorArticleUpdate;

        public ArticlesController(IArticleBLL articleBLL,
            IValidator<ArticleCreateDTO> validatorArticleCreate,
            IValidator<ArticleUpdateDTO> validatorArticleUpdate)
        {
            _articleBLL = articleBLL;
            _validatorArticleCreate = validatorArticleCreate;
            _validatorArticleUpdate = validatorArticleUpdate;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> Get()
        {
            var articles = await _articleBLL.GetArticleWithCategory();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> Get(int id)
        {
            var article = await _articleBLL.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ArticleCreateDTO articleCreateDTO)
        {
            if (articleCreateDTO == null)
            {
                return BadRequest();
            }

            var validatorResult = await _validatorArticleCreate.ValidateAsync(articleCreateDTO);
            if (!validatorResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _articleBLL.Insert(articleCreateDTO);
                return Ok("Insert data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ArticleUpdateDTO articleUpdateDTO)
        {
            if (id != articleUpdateDTO.ArticleID)
            {
                return BadRequest();
            }

            var validatorResult = await _validatorArticleUpdate.ValidateAsync(articleUpdateDTO);
            if (!validatorResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _articleBLL.Update(id, articleUpdateDTO);
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
            var article = await _articleBLL.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                await _articleBLL.Delete(id);
                return Ok("Delete data success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCountArticles()
        {
            try
            {
                var count = await _articleBLL.GetCountArticles();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetWithPaging(int categoryId, int pageNumber, int pageSize)
        {
            try
            {
                var articles = await _articleBLL.GetWithPaging(categoryId, pageNumber, pageSize);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticleByCategory(int categoryId)
        {
            var articles = await _articleBLL.GetArticleByCategory(categoryId);
            if (articles == null)
            {
                return NotFound();
            }
            return Ok(articles);
        }
    }
}
