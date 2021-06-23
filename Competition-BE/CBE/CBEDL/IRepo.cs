using CBEModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEDL
{
    public interface IRepo
    {
        // <summary>
        /// Get a user based on username and email
        /// </summary>
        /// <param name="userName">username of user</param>
        /// <param name="email">email of user</param>
        /// /// <returns>User with the given username and email</returns>
        Task<User> GetUser(string auth0id);
        /// <summary>
        /// Return a user based on id
        /// </summary>
        /// <param name="id">Id of requested yser</param>
        /// <returns>User Associated Id</returns>
        Task<User> GetUser(int id);
        /// <summary>
        /// Gets all users in the database
        /// </summary>
        /// <returns>List of Users</returns>
        Task<List<User>> GetAllUsers();
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">user to add</param>
        /// <returns>user aded</returns>
        Task<User> AddUser(User user);
        /// <summary>
        /// Adds a category to the database
        /// </summary>
        /// <param name="cat">category to be added</param>
        /// <returns>category added to the database</returns>
        Task<Category> AddCategory(Category cat);
        /// <summary>
        /// Retrieves all categories currently in the database
        /// </summary>
        /// <returns>List of categories found</returns>
        Task<List<Category>> GetAllCategories();
        /// <summary>
        /// Gets a category by it's Octokit.Language int name
        /// </summary>
        /// <param name="name">name of category requested</param>
        /// <returns>Category requested</returns>
        Task<Category> GetCategoryByName(int name);
        /// <summary>
        /// Creates a competition and then sends back the ID of the competition for
        /// front end processing
        /// </summary>
        /// <param name="comp">Competition to be added to the database</param>
        /// <returns>int of the competitionId, -1 if failed</returns>
        Task<int> AddCompetition(Competition comp);
        /// <summary>
        /// Gets a competition string based on competition id sent into the method
        /// </summary>
        /// <param name="compId">id of competition to participate in</param>
        /// <returns>string of competition</returns>
        Task<string> GetCompetitionString(int compId);
        /// <summary>
        /// Returns rankings of every User participating in the given competition
        /// </summary>
        /// <param name="compId">Id of competition to get stats from</param>
        /// <returns>List of revelant user scores from competition</returns>
        Task<List<CompetitionStat>> GetCompStats(int compId);
        /// <summary>
        /// Adds the competitionStat to the db, null on fail
        /// </summary>
        /// <param name="c">CompetitionStat to be added</param>
        /// <returns>null on fail, competitionstat on success</returns>
        Task<CompetitionStat> AddCompStat(CompetitionStat c);
        /// <summary>
        /// Updates the given competition stat in the database
        /// </summary>
        /// <param name="c">competition stat to be added</param>
        /// <returns>null on fail, competitionstat on success</returns>
        Task<CompetitionStat> UpdateCompStat(CompetitionStat c);
        /// <summary>
        /// Returns all competitions in db
        /// </summary>
        /// <returns>Returns all competitions in the database</returns>
        Task<List<Competition>> GetAllCompetitions();
        /// <summary>
        /// Gets a category by its id in the database
        /// </summary>
        /// <param name="id">category id to get category from </param>
        /// <returns>Category with id</returns>
        Task<Category> GetCategoryById(int id);
        /// <summary>
        /// Gets all the necessary things for a user to participate in a competition
        /// </summary>
        /// <param name="compId">competition Id for user to get stuff from</param>
        /// <returns>tuple with author, test, category of competition/null on fail</returns>
        Task<Tuple<string, string, int>> GetCompStuff(int compId);
        /// <summary>
        /// Gets a competition by its comp id
        /// </summary>
        /// <param name="id">Id of competition to get</param>
        /// <returns>Competition or null on fail</returns>
        Task<Competition> GetCompetition(int id);
        /// <summary>
        /// Adds a user to the whitelist if possible
        /// </summary>
        /// <param name="compId">competition Id to add to whitelist of</param>
        /// <param name="userId">user Id to add to be added</param>
        /// <returns>true on success, false otherwise</returns>
        Task<bool> WhiteListUser(int compId, int userId);
        /// <summary>
        /// Returns a list of invited participants for a given competition
        /// </summary>
        /// <param name="compId">Id for competition to search for</param>
        /// <returns>List of invited participants, empty list if not found</returns>
        Task<List<InvitedParticipant>> GetInvitedParticipants(int compId);
        /// <summary>
        /// Checks to make sure the given user is on the white list for a restricted competition
        /// </summary>
        /// <param name="compId">Id for competition</param>
        /// <param name="userId">Id for user to check</param>
        /// <returns>True on success, false otherwise</returns>
        Task<bool> CheckTheList(int compId, int userId);
    }
}
