using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Test
{
    public class TestModel
    {
        public int Id { get; set; }
        public TestType Type { get; set; }
        public string Name { get; set; }
    }
    public enum TestType
    {
        Mathematic = 1,
        Science = 2,
        History = 3,
        Geographic = 4,
        Social = 5,
        English = 6
    }
}
