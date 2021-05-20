using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProgrammersBlog.Shared.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly ILogger _logger;

        public MvcExceptionFilter(IHostEnvironment environment, IModelMetadataProvider metadataProvider, ILogger<MvcExceptionFilter> logger)
        {
            _environment = environment;
            _metadataProvider = metadataProvider;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (_environment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                var mvcErrorModel = new MvcErrorModel();
                
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message = $"Üzgünüz, işleminiz sırasında beklenmedik bir veritabanı hatası oluştu. Sorunu en kısa sürede çözeceğiz.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        //ViewResult result1 = new ViewResult { ViewName = "Error" }; bu şekilde ayrı bir view ile de gösterilebilir.
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;

                    case NullReferenceException:
                        mvcErrorModel.Message = $"Üzgünüz, işleminiz sırasında beklenmedik bir referans hatası oluştu. Sorunu en kısa sürede çözeceğiz.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;

                    default:
                        mvcErrorModel.Message = $"Üzgünüz, işleminiz sırasında beklenmedik bir hata oluştu. Sorunu en kısa sürede çözeceğiz.";
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                }
                var result = new ViewResult { ViewName = "Error" };
                result.StatusCode = 500;
                result.ViewData = new ViewDataDictionary(_metadataProvider, context.ModelState);
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);
                context.Result = result;

            }
        }
    }
}
