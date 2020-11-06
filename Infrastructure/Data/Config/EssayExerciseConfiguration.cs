using ApplicationCore.Entities.EssayExerciseAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class EssayExerciseConfiguration : IEntityTypeConfiguration<EssayExercise>
    {
        public void Configure(EntityTypeBuilder<EssayExercise> builder)
        {
            builder.ToTable(nameof(EssayExercise));

        }
    }
}
