using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class TypeTestInput 
    {
        public TypeTestInput() { }
        public int categoryId { get; set; }
        public int numberofcharacters { get; set; }
        public int numberoferrors { get; set; }
        public int wpm { get; set; }
        public int timetakenms { get; set; }
        public DateTime date { get; set; }
    }
}
