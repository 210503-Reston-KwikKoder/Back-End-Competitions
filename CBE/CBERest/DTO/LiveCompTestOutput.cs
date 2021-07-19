using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class LiveCompTestOutput
    {
        public LiveCompTestOutput() { }
        public int CompId { get; set; }
        public string TestString { get; set; }
        public string TestAuthor { get; set; }
        public int Category { get; set; }
    }
}
