using System;
using System.Collections.Generic;


namespace CBERest.DTO{

    public class CompetitionContent{

        public CompetitionContent() { }

        public int id { get; set; }
        public string testString { get; set; }
        public string author { get; set; }
        public int categoryId { get; set; }
    }

}