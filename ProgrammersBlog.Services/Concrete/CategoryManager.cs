﻿using AutoMapper;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Utilities.Constants.Messages;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class CategoryManager : ManagerBase, ICategoryService
    {
       
 
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper)
        {
            
        }

        /// <summary>
        /// Verilen CategoryAddDto ve CreatedByName parametrelerine ait bilgiler ile yeni bir Category ekler.
        /// </summary>
        /// <param name="categoryAddDto">categoryAddDto tipinde eklenecek kategori bilgileri</param>
        /// <param name="createdByName">string tipinde kullanıcının kullanıcı adı</param>
        /// <returns>Asenkron bir operasyon  ile Task olarak ekleme işleminin sonucunu DataResult olarak döner.</returns>
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = Mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory = await UnitOfWork.Categories.AddAsync(category);


            await UnitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Added(categoryAddDto.Name), new CategoryDto {
            
            Category=addedCategory,
            ResultStatus=ResultStatus.Success,
            Message= Messages.Category.Added(categoryAddDto.Name)

            } );
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync();

            if(categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }

            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(c=>!c.IsDeleted);

            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }

            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                var deletedCategory =
                await UnitOfWork.Categories.UpdateAsync(category);
                await UnitOfWork.SaveAsync();


                return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Deleted(deletedCategory.Name), new CategoryDto
                {

                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Deleted(deletedCategory.Name)

                });
            }

            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound, new CategoryDto
            {

               
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.NotFound

            });
        }

        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {
          var category =  await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId,c=>c.Articles);

            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {

                    Category = category,
                    ResultStatus = ResultStatus.Success


                });
            }

            else
            {
               return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound, new CategoryDto { 
               Category=null,
               Message= Messages.Category.NotFound,
               ResultStatus  = ResultStatus.Error
               });
            }
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(null);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto> (ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFoundAny, new CategoryListDto { 
            Categories=null,
            ResultStatus=ResultStatus.Error,
            Message= Messages.Category.NotFoundAny

            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted);


            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFoundAny, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFoundAny

            });



        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive);

            if (categories.Count >= 0)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });

            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFoundAny, null);
        }
        /// <summary>
        /// Verilen Id parametresine ait kategorinin CategoryUpdateDto temsilini geriye döner.
        /// </summary>
        /// <param name="categoryId">0 dan büyük int bir Id değeri</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucunu DataResult tipinde geriye döner.</returns>

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId);

            if(result)
            {
                var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = Mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }

            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, Messages.Category.NotFound, null);
        }

        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if (category != null)
            {

                await UnitOfWork.Categories.DeleteAsync(category);
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success,Messages.Category.HardDeleted(category.Name));
            }

            return new DataResult<Category>(ResultStatus.Error, Messages.Category.NotFound, null);
        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category =Mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);
            category.ModifiedByName = modifiedByName;
            var updatedCategory =
            await UnitOfWork.Categories.UpdateAsync(category);
            await UnitOfWork.SaveAsync();


            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Updated(categoryUpdateDto.Name), new CategoryDto
            {

                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Updated(categoryUpdateDto.Name)
            });

        }

      
    }
}
