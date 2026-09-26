using FluentValidation;
using LearnSphere.Shared.DTOs.LessonDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Validations.LessonValidation
{
    public class CreateLessonValidator:AbstractValidator<CreateLessonDTO>
    {
        public CreateLessonValidator()
        {
            RuleFor(x=>x.SectionId).NotEmpty().WithMessage("Selection The Section");
        }
    }
}
