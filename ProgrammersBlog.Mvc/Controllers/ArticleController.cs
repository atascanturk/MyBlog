using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Mvc.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage =1 ,int pageSize=5, bool isAscending = false)
        {
            var searchedResult = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            if (searchedResult.ResultStatus==ResultStatus.Success)
            {
                return View(new ArticleSearchViewModel
                {
                    ArticleListDto = searchedResult.Data,
                    Keyword = keyword
                });
            }

            return NotFound();

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int articleId)
        {
            var articleResult = await _articleService.GetAsync(articleId);

            if (articleResult.ResultStatus == ResultStatus.Success)
            {
                var userArticles = await _articleService.GetAllByUserIdOnFilter(articleResult.Data.Article.UserId, FİlterBy.Category, OrderBy.Date, false, 10, articleResult.Data.Article.CategoryId, DateTime.Now, DateTime.Now, 0, 99999, 0, 99999);
                await _articleService.IncreaseViewCountAsync(articleId);
                return View(new ArticleDetailViewModel { 
                
                    ArticleDto  = articleResult.Data,
                    ArticleDetailRightSideBarViewModel = new ArticleDetailRightSideBarViewModel
                    {
                        ArticleListDto = userArticles.Data,
                        Header ="Yazarın Aynı Kategorideki En Çok Okunan Makaleleri",
                        User=articleResult.Data.Article.User
                    }
                });
            }

            return NotFound();
        }
    }
}
