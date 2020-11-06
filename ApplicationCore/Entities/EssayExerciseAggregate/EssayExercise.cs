using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.EssayExerciseAggregate
{
    public class EssayExercise : BaseEntity, IAggregateRoot
    {
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public string Title { get; set; }
        public string Result { get; set; }
    }
}
