using FluentValidation;
using LearnSphere.Shared.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Validations.AuthValidation
{
    public class RegisterValidation :AbstractValidator<RegisterDTO>
    {
        public RegisterValidation()
        {
            RuleFor(u=>u.FullName).NotEmpty()
                .WithMessage("Name is required");
            RuleFor(u=>u.Email).EmailAddress()
                .WithMessage("Entre valid Email");

            RuleFor(u => u.Password).NotEmpty()
                .WithMessage("Password required")
                .MinimumLength(5).WithMessage("password must be at least 5 characters");
            RuleFor(u=>u.ConfirmPassword).NotEmpty()
                .WithMessage("please confirm your password")
               
                .Equal(u => u.Password)
                .WithMessage("Password must match");
        }
    }
}
