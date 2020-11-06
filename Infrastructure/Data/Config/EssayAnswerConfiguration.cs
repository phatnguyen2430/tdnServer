using ApplicationCore.Entities.EssayExerciseAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class EssayAnswerConfiguration: IEntityTypeConfiguration<EssayAnswer>
    {
        public void Configure(EntityTypeBuilder<EssayAnswer> builder)
        {
            builder.ToTable(nameof(EssayAnswer));

        }
    }
}
