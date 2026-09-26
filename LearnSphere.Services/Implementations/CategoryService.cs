using AutoMapper;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Implementations;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Repo.Specifications;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unit;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public CategoryService(IUnitOfWork unit,ILogger<CategoryService> logger,
            IMapper mapper,IFileService fileService)
        {
            _unit = unit;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<Result<IReadOnlyList<CategoryDTO>>> GetAllAsync()
        {
            var spec = new BaseSpecification<Category>();
            spec.ApplyNoTracking();
            var categories = await _unit.Repository<Category>()
                .SelectAsync(spec,s=> new CategoryDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                     ImageURL=s.ImageURL,
                    Description=s.Description
                });
          
            return Result<IReadOnlyList<CategoryDTO>>.Success(categories);
           
        }

        public async Task<Result<string>> AddAsync(CreateCategoryDTO categoryDTO)
        {
            var imagePathForDB =await _fileService.UploadImageAsync(categoryDTO.File, "Category");

            Category category = _mapper.Map<Category>(categoryDTO);
          
            category.ImageURL = imagePathForDB.Value;
          // _unit.categories.AddAsync(category);
            await _unit.Repository<Category>().AddAsync(category);
          int NumOfEffectedRows= await _unit.CompleteAsync();
            if(NumOfEffectedRows<=0)
            {
                _logger.LogWarning("No Effected Rows when adding category");
                //return Result<string>.Failure("No Effected Rows when adding category", 404);
                throw new Exception("No Effected Rows when adding category");
            }
            return Result<string>.Success(category.Id, 201);
           
        }

       public async Task<Result<CategoryDTO>> GetById(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return Result<CategoryDTO>.Failure("Id is null or empty",400);

            var cat =await _unit.Repository<Category>().GetByIdAsync(Id,true);

            if (cat == null)
                throw new NotFoundException("category not found");
            // return Result<CategoryDTO>.Failure("category not found", 404);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(cat);
            return Result<CategoryDTO>.Success(categoryDTO);
        }

        public async Task<Result> UpdateAsync(UpdateCategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
                return Result.Failure("put the new values");
            if(string.IsNullOrEmpty(categoryDTO.Id))
                return Result.Failure("Invalid Id");
            var imagePathForDB = await _fileService.UploadImageAsync(categoryDTO.File, "Category");

            var cat =await _unit.Repository<Category>().GetByIdAsync(categoryDTO.Id);
            if(cat==null)
                return Result.Failure("Category Not Found",404);
            cat.ImageURL = imagePathForDB.Value;
            cat.Name = categoryDTO.Name;
            cat.Description = categoryDTO.Description;
          int EffectedRows= await _unit.CompleteAsync();
            if (EffectedRows <= 0)
            {
                _logger.LogError("Failed to update category with id '{}' ",cat.Id);
                return Result.Failure("Server error No Effected Rows", 500);
            }
            return Result.Success();


        }
        public async Task<Result> RemoveAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result.Failure("You must entre the id");
          var cat=  await _unit.Repository<Category>().GetByIdAsync(id);
            if(cat==null)
                return Result.Failure("Category not found",404);

            await _unit.Repository<Category>().RemoveAsync(cat);
            int EffectedRows = await _unit.CompleteAsync();
            if (EffectedRows <= 0)
            {
                _logger.LogError("Failed to Remove category with id '{}' ", cat.Id);
                return Result.Failure("Server error No Effected Rows", 500);
            }
            return Result.Success();

        }

    }
}
