using System;
using System.Collections.Generic;


namespace CBERest.DTO{

    public class CompetitionObject
    {
        
        public CompetitionObject() { }
        public CompetitionObject(string name, DateTime start, DateTime end, int category) {
            this.Name = name;
            this.Start = start;
            this.End = end;
            this.Category = category;
        }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Category { get; set; }
        public string snippet { get; set; }
        public string author { get; set; }
        public int compId { get; set; }
    }

}