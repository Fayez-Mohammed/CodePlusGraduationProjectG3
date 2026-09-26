using LearnSphere.DAL.Models;
using LearnSphere.DAL.Models.BaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.DAL.Configration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser> 
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserType).HasConversion<string>();
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Bio).HasMaxLength(1000);
            builder.Property(x => x.ImagePath).HasMaxLength(500);
            
        }
    }
}
