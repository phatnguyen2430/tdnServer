using ApplicationCore.Entities.TestAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable(nameof(Test));
            builder.HasMany(x => x.MultipleChoicesExercises).WithOne(x => x.Test).HasForeignKey(x => x.TestId);
            builder.HasMany(x => x.EssayExercises).WithOne(x => x.Test).HasForeignKey(x => x.TestId);
            builder.HasMany(x => x.Answers).WithOne(x => x.Test).HasForeignKey(x => x.TestId);

        }
    }
}
