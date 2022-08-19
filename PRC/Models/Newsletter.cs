using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Newsletter
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
