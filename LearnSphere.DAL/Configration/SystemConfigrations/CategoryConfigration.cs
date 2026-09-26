using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class CategoryConfigration:BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(500);
            
           
        }
    }
}
