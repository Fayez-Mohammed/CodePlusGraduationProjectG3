using FluentValidation;
using LearnSphere.Shared.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Validations.CategoryValidation
{
    public class CreateCategoryValidator:AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c=>c.Name).Must(name=>!string.IsNullOrWhiteSpace(name)).WithMessage("Name is required");
            RuleFor(c => c.Description).MaximumLength(500).WithMessage("max length is 500");
        }
    }
}
