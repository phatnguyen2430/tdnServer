using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.EssayAnswer;
using WebAPI.Models.MultipleChoicesAnswer;

namespace WebAPI.Models.Answer
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int StudentId { get; set; }
    }
    public class AnswerContainerModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int StudentId { get; set; }
        public List<MultipleChoicesAnswerModel> MultipleChoicesAnswerModels { get; set; }
        public List<EssayAnswerModel> EssayAnswerModels { get; set; }
    }
}
