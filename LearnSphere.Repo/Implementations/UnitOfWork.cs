using LearnSphere.DAL.Context;
using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Repo.Implementations
{
    public  class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;
        Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }
        public  IGenericRepo<T> Repository<T>() where T : BaseEntity
        {
            var type=typeof(T);
            if (!repositories.ContainsKey(type))
            {
                var repo =new GenericRepo<T>(_context);
                repositories.Add(type, repo);

            }

          return (IGenericRepo<T>) repositories[type];
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
//using UniversityManagement.API.Context;
//using UniversityManagement.API.Models;
//using UniversityManagement.API.Repository;
//using UniversityManagement.API.Repository.Interfaces;
//namespace UniversityManagement.API.UnitOfWork
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly UniversityDbContext _context;
//        public IStudentRepository Students { get; }
//        public IGenericRepository<Course> Courses { get; }
//        public IDepartmentRepository Departments { get; }
//        public IGenericRepository<Enrollment> Enrollments { get; }
//        public UnitOfWork(UniversityDbContext context)
//        {
//            _context = context;
//            Students = new StudentRepository(_context);
//            Courses = new GenericRepository<Course>(_context);
//            Departments = new DepartmentRepository(_context);
//        }

//        public async Task SaveAsync()
//        {
//            await _context.SaveChangesAsync();
//        }

//    }
//}