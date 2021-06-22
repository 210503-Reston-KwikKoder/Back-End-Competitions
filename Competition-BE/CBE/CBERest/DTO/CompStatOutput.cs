using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class CompStatOutput
    {
        public CompStatOutput() { }
        public double wpm { get; set; }
        public int rank { get; set; }
        public string userName { get; set; }
        public double accuracy { get; set; }
        public string Name { get; set; }
        public string CompName { get; set; }
        public int userId { get; set; }
    }
}
