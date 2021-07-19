using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEModels
{
    public class UserQueue
    {
        public UserQueue() { }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public LiveCompetition LiveCompetition { get; set; }
        [ForeignKey("LiveCompetition")]
        public int LiveCompetitionId { get; set; }
        public DateTime EnterTime { get; set; }
    }
}
