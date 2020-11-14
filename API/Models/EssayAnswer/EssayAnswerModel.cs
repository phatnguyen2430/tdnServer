using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.EssayAnswer
{
    public class EssayAnswerResponseModel
    {
        public int Id { get; set; }
        public int EssayExerciseId { get; set; }
        public int AnswerId { get; set; }
        public string Result { get; set; }
        public bool IsBingo { get; set; }
    }
    public class EssayAnswerRequestModel
    {
        public int EssayExerciseId { get; set; }
        public int AnswerId { get; set; }
        public string Result { get; set; }
        public bool IsBingo { get; set; }
    }
}
