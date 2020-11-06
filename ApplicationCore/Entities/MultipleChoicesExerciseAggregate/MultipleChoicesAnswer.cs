using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.MultipleChoicesExerciseAggregate
{
    public class MultipleChoicesAnswer : BaseEntity, IAggregateRoot
    {
        //public virtual MultipleChoicesExercise EssayExercise { get; set; }
        public int MultipleChoicesExerciseId { get; set; }
        public virtual Answer Answer { get; set; }
        public int AnswerId { get; set; }
        public string Result { get; set; }
    }
}
