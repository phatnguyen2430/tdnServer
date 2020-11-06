using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Answer
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int StudentId { get; set; }
    }
}
