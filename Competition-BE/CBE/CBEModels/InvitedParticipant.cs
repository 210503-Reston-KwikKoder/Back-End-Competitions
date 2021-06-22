using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEModels
{
    public class InvitedParticipant
    {
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
