using FluentValidation;
using LearnSphere.Shared.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Validations.CourseValidation
{
    public class CreateCourseValidator:AbstractValidator<CreateCourseDTO>
    {
        public CreateCourseValidator()
        {
            RuleFor(x=>x.Title).Must(title=>!string.IsNullOrWhiteSpace(title)).WithMessage("Title is required");
        }
    }
    public class UpdateCourseValidator:AbstractValidator<UpdateCourseDTO>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x=>x.Title).Must(title=>!string.IsNullOrWhiteSpace(title)).WithMessage("Title is required");
        }
    }
}
