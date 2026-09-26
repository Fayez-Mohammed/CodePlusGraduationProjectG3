using FluentValidation;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Validations
{
    public class UpdateCategoryValidator:AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c=>c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Description).MaximumLength(500).WithMessage("max length is 500");

        }
    }
}
