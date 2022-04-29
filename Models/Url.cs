using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyUrl.Models
{
    public class Url
    {      
        public string Id { get; set; }
        public string Key { get; set; } 
        public string FullUrl { get; set; } 
        public DateTime CreationTime { get; set; }
        public int UsageCount { get; set; }
    }
}

