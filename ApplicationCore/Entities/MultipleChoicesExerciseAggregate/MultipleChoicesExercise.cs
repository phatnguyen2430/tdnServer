using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.MultipleChoicesExerciseAggregate
{
    public class MultipleChoicesExercise : BaseEntity, IAggregateRoot
    {
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public string Title { get; set; }
        public string RightResult { get; set; }
        public string FalseResult1 { get; set; }
        public string FalseResult2 { get; set; }
        public string FalseResult3 { get; set; }
        public byte[] Image { get; set; }
    }
}
