using CBEModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEBL
{
    public interface ICompBL
    {
        /// <summary>
        /// Adds a competition to the database given the required fields
        /// </summary>
        /// <param name="startDate">Date the Comp started</param>
        /// <param name="endDate">Date the Comp ended</param>
        /// <param name="categoryId">Category for competiton</param>
        /// <param name="competitionName">name for the competition</param>
        /// <param name="user">authId for user starting the competition</param>
        /// <param name="teststring">test string for the competition to be added</param>
        /// <returns>Tuple with int for comp id and string of code to be competed on</returns>
        Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int user, string teststring);
        /// <summary>
        /// Adds a competition to the database given the required fields
        /// </summary>
        /// <param name="startDate">Date the Comp started</param>
        /// <param name="endDate">Date the Comp ended</param>
        /// <param name="categoryId">Category for competiton</param>
        /// <param name="competitionName">name for the competition</param>
        /// <param name="user">authId for user starting the competition</param>
        /// <param name="teststring">test string for the competition to be added</param>
        /// <param name="author">test string for the competition to be added</param>
        /// <returns>Tuple with int for comp id and string of code to be competed on</returns>
        Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int user, string teststring, string author);
        /// <summary>
        /// Method which returns the users that participated in a given competition
        /// </summary>
        /// <param name="competitionId"></param>
        /// <returns></returns>
        Task<List<CompetitionStat>> GetCompetitionStats(int competitionId);
        /// <summary>
        /// Adds a competition to the database and updates the rankings
        /// </summary>
        /// <param name="competitionStat">Competition stat to be added</param>
        /// <param name="numberWords">number of words in test</param>
        /// <param name="numberErrors">number of errors in test</param>
        /// <returns>rank in the competition, -1 on error</returns>
        Task<int> InsertCompStatUpdate(CompetitionStat competitionStat, int numberWords, int numberErrors);
        /// <summary>
        /// Method that returns the string for the given competition
        /// </summary>
        /// <param name="compId">competition id to get</param>
        /// <returns>string to be competed upon</returns>
        Task<string> GetCompString(int compId);
        /// <summary>
        /// Method that returns all the competitions in the database
        /// </summary>
        /// <returns>List of Competitions in database</returns>
        Task<List<Competition>> GetAllCompetitions();
        /// <summary>
        /// Gets all the necessary things for a user to participate in a competition
        /// </summary>
        /// <param name="compId">competition id to get things from</param>
        /// <returns>tuple with author, test, category of competition/null on fail</returns>
        Task<Tuple<string, string, int>> GetCompStuff(int compId);
        /// <summary>
        /// Gets a competition by id, null if not found
        /// </summary>
        /// <param name="compId">id of competition to be found</param>
        /// <returns>Competition or null if not found</returns>
        Task<Competition> GetCompetition(int compId);
        /// <summary>
        /// Adds a live competition to the database and returns the new id
        /// </summary>
        /// <param name="liveCompetition">LiveCompetition to be added</param>
        /// <returns>-1 on error, new live competition id otherwise</returns>
        Task<int> AddLiveCompetition(LiveCompetition liveCompetition);
        /// <summary>
        /// Gets a live competition by the Id
        /// </summary>
        /// <param name="id">id of live competition to get</param>
        /// <returns>Live Competition associated with the given ID</returns>
        Task<LiveCompetition> GetLiveCompetition(int id);
        /// <summary>
        /// Adds a Live Competition Test to a given competition, has to reference a given live competition
        /// </summary>
        /// <param name="liveCompetitionTest">live competition to be added</param>
        /// <returns>LiveCompetitionTest added, null on error</returns>
        Task<LiveCompetitionTest> AddLiveCompetitionTest(LiveCompetitionTest liveCompetitionTest);
        /// <summary>
        /// Gets a list of tests associated with a given competition
        /// </summary>
        /// <param name="compId">Id of competition to get tests for</param>
        /// <returns>List of CompetitionTests associated with competition, empty if none found</returns>
        Task<List<LiveCompetitionTest>> GetLiveCompetitionTestsForCompetition(int compId);
        /// <summary>
        /// Method to get all the live competitions in the database
        /// </summary>
        /// <returns>List of live competitions, empty if none exist</returns>
        Task<List<LiveCompetition>> GetLiveCompetitions();
        /// <summary>
        /// Adds a given UserQueue to the database
        /// </summary>
        /// <param name="userQueue">UserQueue to be added</param>
        /// <returns>UserQueue added or null on error</returns>
        Task<UserQueue> AddToQueue(UserQueue userQueue);
        /// <summary>
        /// Gets all the associated UserQueue items with a given live competition
        /// </summary>
        /// <param name="liveCompId">Live competition Id in order to find the user queue</param>
        /// <returns>List of user queue objects for the associated live competition, empty if none found</returns>
        Task<List<UserQueue>> GetLiveCompetitionUserQueue(int liveCompId);
        /// <summary>
        /// Dequeues the oldest user in queue for competition and returns him or her
        /// </summary>
        /// <returns>Oldest UserQueue for a given </returns>
        Task<UserQueue> DeQueueUserQueue(int liveCompId);
        /// <summary>
        /// Deletes a given user from a given live competition
        /// </summary>
        /// <param name="liveCompId">Id of live competition to delete user from</param>
        /// <param name="UserId">Id of user to be deleted</param>
        /// <returns>deleted user or null if not found</returns>
        Task<UserQueue> DeleteUserFromQueue(int liveCompId, int userId);
        /// <summary>
        /// Adds new or updates existing live comp stat in db for given comp, user, and outcome
        /// </summary>
        /// <param name="liveCompId">compid of live competition</param>
        /// <param name="userId">id of user in competition</param>
        /// <param name="won">true if user won/false otherwise</param>
        /// <returns>livecompstat created or updated</returns>
        Task<LiveCompStat> AddUpdateLiveCompStat(int liveCompId, int userId, bool won);
        /// <summary>
        /// Gets a list of live competition stat for a a given livecompid ordered by wins
        /// </summary>
        /// <param name="liveCompId">competition id of livecompstats</param>
        /// <returns>Live competition stats for a given competition</returns>
        Task<List<LiveCompStat>> GetLiveCompStats(int liveCompId);
    }
}
