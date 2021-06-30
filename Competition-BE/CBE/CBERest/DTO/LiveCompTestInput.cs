using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class LiveCompTestInput
    {
        public LiveCompTestInput() { }
        public int compId { get; set; }
        public int category { get; set; }
        public string testString { get; set; }
        public string testAuthor { get; set; }
    }
}
