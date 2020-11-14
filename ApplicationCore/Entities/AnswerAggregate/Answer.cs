using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.AnswerAggregate
{
    public class Answer : BaseEntity, IAggregateRoot
    {
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Test Test { get; set; }
    }
}
