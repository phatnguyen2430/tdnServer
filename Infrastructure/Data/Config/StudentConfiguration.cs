using ApplicationCore.Entities.StudentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(nameof(Student));
            //builder.HasMany(x => x.Annotations).WithOne(x => x.Student).HasForeignKey(x=>x.StudentId);
            //builder.HasMany(x => x.Answers).WithOne(x => x.Student).HasForeignKey(x=>x.StudentId);
        }
    }
}
