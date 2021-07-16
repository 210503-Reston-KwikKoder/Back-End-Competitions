using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class QueueModel
    {
        public QueueModel() { }
        public string userId { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public DateTime enterTime { get; set; }
    }
}
