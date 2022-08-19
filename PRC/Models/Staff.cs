using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models
{
    public class Staff
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Department { get; set; }
        public string Role { get; set; }
        public bool Lead { get; set; }
        public string Image { get; set; }
    }
}
