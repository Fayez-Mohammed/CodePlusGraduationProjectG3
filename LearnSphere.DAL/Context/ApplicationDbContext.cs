using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace LearnSphere.DAL.Context
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom((entityType.ClrType)) && !entityType.IsOwned())
                {
                    var method = typeof(ApplicationDbContext).GetMethod(nameof(ConfigureSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);
                    method.Invoke(null, new object[] { builder });
                }
            }
        }
        private static void ConfigureSoftDeleteFilter<TEntity>(ModelBuilder builder)where TEntity :class,ISoftDelete
        {
            builder.Entity<TEntity>().HasQueryFilter(e=>!e.IsDeleted);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableFields();
            SoftDeleteFields();
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            UpdateAuditableFields();
            SoftDeleteFields();
            return base.SaveChanges();
        }
        private void UpdateAuditableFields()
        {
            var entries = ChangeTracker.Entries<IAuditableEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
        private void SoftDeleteFields()
        {
            var entries = ChangeTracker.Entries<ISoftDelete>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }
        }
        
        #region DbSets

        // 1. Identity & Tokens
        //public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        // 2. Course Management & Content
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Section> Sections { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;

        // 3. Enrollments & Learning Progress
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<LessonProgress> LessonProgresses { get; set; } = null!;

        // 4. Quiz System
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;
        public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;

        // 5. Certificates, Reviews & Wishlist
        public DbSet<Certificate> Certificates { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;

        // 6. Payments & Coupons
        public DbSet<Coupon> Coupons { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;

        #endregion
    }
}
