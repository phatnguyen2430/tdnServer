using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.EssayExerciseAggregate
{
    public class EssayAnswer : BaseEntity, IAggregateRoot
    {
        //public virtual EssayExercise EssayExercise { get; set; }
        public int EssayExerciseId { get; set; }
        public virtual Answer Answer { get; set; }
        public int AnswerId { get; set; }
        public string Result { get; set; }
        public string ResultFixed { get; set; }
        public bool IsBingo { get; set; }
    }
}
