using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.TestAggregate
{
    public class Test : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public virtual List<MultipleChoicesExercise> MultipleChoicesExercises { get; set; }
        public virtual List<EssayExercise> EssayExercises { get; set; }
        //public int StudentId { get; set; }
        //public virtual Student Student { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public TestType Type { get; set; }
    }
    public enum TestType
    {
        Mathematic = 1,
        Science = 2,
        History = 3,
        Geographic = 4,
        Social = 5,
        English = 6,
        AssTest = 7
    }
}
