﻿using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Models;
using ProgrammersBlog.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.ViewComponents
{
    public class RightSideBarViewComponent :ViewComponent
                  
    {
        ICategoryService _categoryService;
        IArticleService _articleService;

        public RightSideBarViewComponent(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articleResult = await _articleService.GetAllByViewCountAsync(false, 5);

            return View(new RightSideBarViewModel
            {
                Categories = categoriesResult.Data.Categories,
                Articles = articleResult.Data.Articles
            });
        }
    }
}
