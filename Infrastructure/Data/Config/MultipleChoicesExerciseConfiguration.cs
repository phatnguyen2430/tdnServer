using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class MultipleChoicesExerciseConfiguration : IEntityTypeConfiguration<MultipleChoicesExercise>
    {
        public void Configure(EntityTypeBuilder<MultipleChoicesExercise> builder)
        {
            builder.ToTable(nameof(MultipleChoicesExercise));
        }
    }
}
