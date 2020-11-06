using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class MultipleChoicesAnswerConfiguration : IEntityTypeConfiguration<MultipleChoicesAnswer>
    {
        public void Configure(EntityTypeBuilder<MultipleChoicesAnswer> builder)
        {
            builder.ToTable(nameof(MultipleChoicesAnswer));
        }
    }
}
