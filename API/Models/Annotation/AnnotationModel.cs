using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Annotation
{
    public class AnnotationModel
    {
        public int Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int StudentId { get; set; }
        public int Type { get; set; }
    }
}
