using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Log
{
    public class LogModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public bool Status { get; set; }
    }
}
