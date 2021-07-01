using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class LiveCompStatModel
    {
        public LiveCompStatModel() { }
        public int userId { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double WLRatio { get; set; }
    }
}
