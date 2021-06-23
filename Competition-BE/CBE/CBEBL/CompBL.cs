using CBEDL;
using CBEModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEBL
{
    public class CompBL:ICompBL
    {
        private readonly Repo _repo;
        public CompBL(CBEDbContext context)
        {
            _repo = new Repo(context);

        }
        public async Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int user, string teststring)
        {
            Competition competition = new Competition();
            competition.StartDate = startDate;
            competition.EndDate = endDate;
            competition.CategoryId = categoryId;
            competition.CompetitionName = competitionName;
            competition.TestString = teststring;
            competition.UserCreatedId = user;
            return await _repo.AddCompetition(competition);
        }
        public async Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int user, string teststring, string author, bool restricted)
        {
            Competition competition = new Competition();
            competition.StartDate = startDate;
            competition.EndDate = endDate;
            competition.CategoryId = categoryId;
            competition.CompetitionName = competitionName;
            competition.TestString = teststring;
            competition.UserCreatedId = user;
            competition.TestAuthor = author;
            competition.Restricted = restricted;
            return await _repo.AddCompetition(competition);
        }

        public async Task<bool> CheckTheList(int compId, int userId)
        {
            return await _repo.CheckTheList(compId, userId);
        }

        public async Task<List<Competition>> GetAllCompetitions()
        {
            return await _repo.GetAllCompetitions();
        }

        public async Task<Competition> GetCompetition(int compId)
        {
            return await _repo.GetCompetition(compId);
        }

        public async Task<List<CompetitionStat>> GetCompetitionStats(int competitionId)
        {
            return await _repo.GetCompStats(competitionId);
        }

        public async Task<string> GetCompString(int compId)
        {
            return await _repo.GetCompetitionString(compId);
        }

        public async Task<Tuple<string, string, int>> GetCompStuff(int compId)
        {
            return await _repo.GetCompStuff(compId);
        }

        public async Task<List<InvitedParticipant>> GetWhiteList(int compId)
        {
            return await _repo.GetInvitedParticipants(compId);
        }

        public async Task<int> InsertCompStatUpdate(CompetitionStat competitionStat, int numberWords, int numberErrors)
        {
            try
            {
                double numWords = (double)numberWords;
                numWords = numWords / 5;
                double numErrors = (double)numberErrors;
                numErrors = numErrors / 5;
                competitionStat.Accuracy = (numWords - numErrors) / numWords;
                if (await _repo.AddCompStat(competitionStat) == null) throw new ArgumentNullException("Error adding competition stat");
                List<CompetitionStat> competitionStats = await _repo.GetCompStats(competitionStat.CompetitionId);
                int i = 0;
                foreach (CompetitionStat c in competitionStats)
                {
                    i += 1;
                    c.rank = i;
                    await _repo.UpdateCompStat(c);
                }

                return competitionStats.First(comp => comp.UserId == competitionStat.UserId).rank;
            }
            catch (Exception)
            {
                Log.Error("error in insertCompStat returning null");
                return -1;
            }
        }

        public async Task<bool> WhiteListUser(int compId, int userId)
        {
            try
            {
                if (!((await _repo.GetCompetition(compId)).Restricted)) return false;
                else await _repo.WhiteListUser(compId, userId);
                return true;
            }
            catch(Exception e)
            {
                Log.Error(e.StackTrace);
                return false;
            }
        }
    }
}
