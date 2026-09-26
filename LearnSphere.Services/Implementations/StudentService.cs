using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.StudentDTOs;
using LearnSphere.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unit;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentService(IUnitOfWork unit,IUserService userService,UserManager<ApplicationUser> userManager)
        {
            _unit  = unit;
            _userService = userService;
            _userManager = userManager;
        }
     
        public async Task<Result<PagedResult<StudentsDTO>>> GetStudents(string search,int pageNumber,int pageSize)
        {
            var querey = _userManager.Users.Where(u => u.UserType == UserTypes.Student).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
                querey = querey.Where(u => u.FullName.Contains(search));
            int totalCount = querey.Count();

            var items =await  querey.OrderByDescending(x => x.DateOfCreation).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(u => new StudentsDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                ImagePath = u.ImagePath,
                DateOfCreation = u.DateOfCreation,
                EnrolledCoursesCount = u.Enrollments.Count(),
                TotalSpent = u.Payments.Where(x => x.Status == PaymentStatus.Success).Sum(x => (decimal?)x.Amount) ?? 0m
            }).ToListAsync();
           
            var resutlDTO = new PagedResult<StudentsDTO>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
            return Result<PagedResult<StudentsDTO>>.Success(resutlDTO);
            
        }

        public async Task<Result<StudentDetailsDTO>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Result<StudentDetailsDTO>.Failure("Invalid Id");
           var user=await _userManager.Users.Where(u=>u.Id==id).AsNoTracking().Select(u=>new StudentDetailsDTO
           {
               Id=u.Id,
               FullName=u.FullName,
               Email=u.Email, 
               ImagePath=u.ImagePath,
               DateOfCreation=u.DateOfCreation,
               Bio=u.Bio,
               Status =new StudentStatus
               {
                   TotalSpent=u.Payments.Where(p=>p.Status==PaymentStatus.Success).Sum(p=>(decimal?)p.Amount)??0m,
                   CompletedCoursesCount=u.Enrollments.Count(c=>c.IsCompleted),
                   EnrolledCoursesCount=u.Enrollments.Count(),
                   CertificatesEarned=u.Certificates.Count(),
                   WishListCount=u.Wishlists.Count(),
                   ReviewWritten=u.Reviews.Count()
               }
           }).FirstOrDefaultAsync();
            if (user == null)
                throw new NotFoundException("User Not Found");
            return Result<StudentDetailsDTO>.Success(user);
        }
    }
}
