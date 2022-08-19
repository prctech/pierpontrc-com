using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool Answered { get; set; }
        public bool Public { get; set; }
        public Ask Type { get; set; }
    }

    public enum Ask
    { 
        Fitness,
        Specialist
    }
}
