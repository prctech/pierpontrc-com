using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRC.Models.ViewModels
{
    public class PhotosPageViewModel
    {
        public Guid PageId { get; set; }
        public string Content { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
