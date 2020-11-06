using ApplicationCore.Entities.AnswerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable(nameof(Answer));
            //builder.HasMany(x => x.EssayAnswers).WithOne(x => x.Answer).HasForeignKey(x => x.AnswerId);
            //builder.HasMany(x => x.MultipleChoicesAnswers).WithOne(x => x.Answer).HasForeignKey(x => x.AnswerId);
        }
    }
}
