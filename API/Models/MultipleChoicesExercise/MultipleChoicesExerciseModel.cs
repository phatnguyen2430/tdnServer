using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.MultipleChoicesExercise
{
    public class MultipleChoicesExerciseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public string RightResult { get; set; }
        public string FalseResult1 { get; set; }
        public string FalseResult2 { get; set; }
        public string FalseResult3 { get; set; }
    }
}
