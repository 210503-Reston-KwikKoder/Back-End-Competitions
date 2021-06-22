using CBEDL;
using CBEModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CBEBL
{
    public class UserBL : IUserBL
    {
        private readonly Repo _repo;
        public UserBL(CBEDbContext context)
        {
            _repo = new Repo(context);
        }

        public async Task<User> AddUser(User u)
        {
            
            return await _repo.AddUser(u);
            
        }

        public async Task<User> GetUser(int id)
        {
            return await _repo.GetUser(id);
        }

        public async Task<User> GetUser(string userID)
        {
            return await _repo.GetUser(userID);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _repo.GetAllUsers();
        }
    }
}
