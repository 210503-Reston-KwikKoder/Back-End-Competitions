using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class QueueModel
    {
        public QueueModel() { }
        public int userId { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public DateTime enterTime { get; set; }
    }
}
