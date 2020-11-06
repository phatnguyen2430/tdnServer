using ApplicationCore.Entities.StudentAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.AnnotationAggregate
{
    public class Annotation : BaseEntity, IAggregateRoot
    {
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int Type { get; set; }
    }
    public enum AnnotationType
    {
        NewExercise = 1,
        NewResult = 2,
        Expire = 3
    }
}
