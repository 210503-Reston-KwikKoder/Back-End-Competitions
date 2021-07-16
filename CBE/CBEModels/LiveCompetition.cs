using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEModels
{
    public class LiveCompetition
    {
        public LiveCompetition() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LiveCompetitionTest> LiveCompetitionTests { get; set; }
    }
}
