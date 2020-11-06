using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Entities.StudentAggregate;
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
        //public virtual List<MultipleChoicesAnswer> MultipleChoicesAnswers { get; set; }
        //public virtual List<MultipleChoicesAnswer> EssayAnswers { get; set; }
        //public string MultipleChoicesAnswers { get; set; }
        //public string EssayAnswers { get; set; }
        public double Score { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Test Test { get; set; }
    }
}
