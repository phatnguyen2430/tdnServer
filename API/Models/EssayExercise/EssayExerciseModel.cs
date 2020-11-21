using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.EssayExercise
{
    public class EssayExerciseResponseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public string Result { get; set; }
        public string Image { get; set; }
    }
    public class EssayExerciseRequestModel
    {
        public int TestId { get; set; }
        public string Title { get; set; }
        public string Result { get; set; }
        public string Image { get; set; }
    }
}
