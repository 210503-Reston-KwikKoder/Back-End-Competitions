using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBEModels;

namespace CBEBL
{
    public interface IUserBL
    {
        /// <summary>
        /// Method to get users from the database
        /// </summary>
        /// <returns>List of Users in the database</returns>
        Task<List<User>> GetUsers();
        /// <summary>
        /// Method that adds a user to the db if able
        /// </summary>
        /// <param name="u">User to be added to the db</param>
        /// <returns>user added, null otherwise</returns>
        Task<User> AddUser(User u);
        /// <summary>
        /// Get a user by his or her ID
        /// </summary>
        /// <param name="id">ID of requested user</param>
        /// <returns></returns>
        Task<User> GetUser(int id);
        /// <summary>
        /// Get a user by his or her Auth0ID
        /// </summary>
        /// <param name="id">ID of requested user</param>
        /// <returns></returns>
        Task<User> GetUser(string userID);
    }
}
