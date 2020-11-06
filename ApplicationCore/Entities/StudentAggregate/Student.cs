using ApplicationCore.Entities.AnnotationAggregate;
using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Entities.StudentAggregate
{
    public class Student : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Please enter Email.")]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }
        //public List<Test> Tests { get; set; }
        public virtual List<Answer> Answers { get; set; }
        //public virtual Admin Admin { get; set; }
        //public int AdminId { get; set; }
        public virtual List<Annotation> Annotations { get; set; }
        public Student()
        {
            UpdatedOnUtc = DateTime.Now;
            CreatedOnUtc = DateTime.Now;
        }
    }
}