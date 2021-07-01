using CBEModels;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEDL
{
    public class Repo : IRepo
    {
        private readonly CBEDbContext _context;
        public Repo(CBEDbContext context)
        {
            _context = context;
            Log.Debug("Repo instantiated");
        }

        public async Task<Category> AddCategory(Category cat)
        {
            try
            {
                //Make sure category doesn't already exists
                await (from c in _context.Categories
                       where c.Name == cat.Name
                       select c).SingleAsync();
                return null;
            }
            catch (Exception)
            {
                await _context.Categories.AddAsync(cat);
                await _context.SaveChangesAsync();
                Log.Information("New category created.");
                return cat;
            }
        }

        public async Task<int> AddCompetition(Competition comp)
        {
            try
            {
                await _context.Competitions.AddAsync(comp);
                await _context.SaveChangesAsync();
                return comp.Id;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error adding competition returning -1");
                return -1;
            }
        }

        public async Task<CompetitionStat> AddCompStat(CompetitionStat c)
        {
            try
            {
                CompetitionStat cstat = await (from compStat in _context.CompetitionStats
                                               where compStat.UserId == c.UserId
                                               && compStat.CompetitionId == c.CompetitionId
                                               select compStat).SingleAsync();
                cstat.WPM = c.WPM;
                cstat.Accuracy = c.Accuracy;
                await _context.SaveChangesAsync();
                _context.Entry(cstat).State = EntityState.Detached;
                return c;
            }
            catch (Exception) { Log.Information("Could not find stat, adding new entry in competition."); }
            try
            {
                await _context.CompetitionStats.AddAsync(c);
                await _context.SaveChangesAsync();
                return c;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error adding competitionstat returning null");
                return null;
            }
        }

        public async Task<int> AddLiveCompetition(LiveCompetition liveCompetition)
        {
            try
            {
                await _context.LiveCompetitions.AddAsync(liveCompetition);
                await _context.SaveChangesAsync();
                return liveCompetition.Id;

            }catch(Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("Error occured in adding live competition");
                return -1;
            }
        }

        public async Task<LiveCompetitionTest> AddLiveCompetitionTest(LiveCompetitionTest liveCompetitionTest)
        {
            try
            {
                await _context.LiveCompetitionTests.AddAsync(liveCompetitionTest);
                await _context.SaveChangesAsync();
                return liveCompetitionTest;
            }catch(Exception e)
            {
                Log.Error(e.StackTrace);
                return null;
            }
        }

        public async Task<LiveCompStat> AddUpdateLiveCompStat(int liveCompId, int userId, bool won)
        {
            try
            {
                LiveCompStat liveCompStat = await (from lCS in _context.LiveCompStats
                                                   where lCS.LiveCompetitionId == liveCompId
                                                   && lCS.UserId == userId
                                                   select lCS).SingleAsync();
                if (won) liveCompStat.Wins += 1;
                else liveCompStat.Losses += 1;
                double winDouble = (double)liveCompStat.Wins;
                double lossDouble = (double)liveCompStat.Losses;
                liveCompStat.WLRatio = winDouble / (winDouble + lossDouble);
                await _context.SaveChangesAsync();
                return liveCompStat;
            }
            catch (Exception e)
            {
                Log.Information(e.StackTrace);
                Log.Information("Creating new live comp stat");
                LiveCompStat liveCompStat = new LiveCompStat();
                liveCompStat.LiveCompetitionId = liveCompId;
                liveCompStat.UserId = userId;
                if (won)
                {
                    liveCompStat.WLRatio = 1;
                    liveCompStat.Wins = 1;
                    liveCompStat.Losses = 0;
                    await _context.LiveCompStats.AddAsync(liveCompStat);
                    await _context.SaveChangesAsync();
                    return liveCompStat;
                }
                else
                {
                    liveCompStat.WLRatio = 0;
                    liveCompStat.Wins = 0;
                    liveCompStat.Losses = 1;
                    await _context.LiveCompStats.AddAsync(liveCompStat);
                    await _context.SaveChangesAsync();
                    return liveCompStat;
                }
            }
        }

        public async Task<UserQueue> AddToQueue(UserQueue userQueue)
        {
            try
            {
                await _context.UserQueues.AddAsync(userQueue);
                await _context.SaveChangesAsync();
                return userQueue;
            }catch(Exception e)
            {
                Log.Error(e.StackTrace);
                return null;
            }
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                if (await GetUser(user.Auth0Id) != null) return null;
                user.Revapoints = 0;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public async Task<UserQueue> DeleteUserFromQueue(int liveCompId, int userId)
        {
            try {
                UserQueue userQueue = await(from uQ in _context.UserQueues
                                            where uQ.LiveCompetitionId == liveCompId
                                            && uQ.UserId == userId
                                            select uQ).SingleAsync();
                await Task.Run(() =>
                {
                    _context.Remove(userQueue);
                });
                await _context.SaveChangesAsync();
                return userQueue;
            }
            catch (Exception e) {
                Log.Error(e.StackTrace);
                Log.Error("User was not found in live competition");
                return null;
            }
        }

        public async Task<UserQueue> DeQueueUserQueue(int liveCompId)
        {
            try
            {
                UserQueue nextUserInQueue = await (from uQ in _context.UserQueues
                                                   where uQ.LiveCompetitionId == liveCompId
                                                   orderby uQ.EnterTime ascending
                                                   select uQ).FirstAsync();
                await Task.Run(() =>
                {
                    _context.Remove(nextUserInQueue);
                });
                await _context.SaveChangesAsync();
                return nextUserInQueue;
            }
            catch(Exception e)
            {
                Log.Error("Empty Queue for competition");
                Log.Error(e.StackTrace);
                return null;
            }
        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                return await (from c in _context.Categories
                              select c).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public async Task<List<Competition>> GetAllCompetitions()
        {
            try
            {
                return await (from c in _context.Competitions
                              select c).ToListAsync();
            }
            catch (Exception)
            {
                Log.Information("No competitions found, returning empty list");
                return new List<Competition>();
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await (from u in _context.Users
                              select u).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return new List<User>();
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                return await (from c in _context.Categories
                              where c.Id == id
                              select c).SingleAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("Error finding category returning null");
                return null;
            }
        }

        public async Task<Category> GetCategoryByName(int name)
        {
            try
            {
                return await (from cat in _context.Categories
                              where cat.Name == name
                              select cat).SingleAsync();
            }
            catch (Exception e)
            {
                Log.Information(e.StackTrace);
                Log.Information("No such category found");
                return null;

            }
        }

        public async Task<Competition> GetCompetition(int id)
        {
            try
            {
                return await (from c in _context.Competitions
                              where c.Id == id
                              select c).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("No competition found");
                return null;
            }
        }

        public async Task<string> GetCompetitionString(int compId)
        {
            try
            {
                return await (from comp in _context.Competitions
                              where comp.Id == compId
                              select comp.TestString).SingleAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("Error retrieving string");
                return null;
            }
        }

        public async Task<List<CompetitionStat>> GetCompStats(int compId)
        {
            try
            {
                return await (from compStat in _context.CompetitionStats
                              where compStat.CompetitionId == compId
                              orderby compStat.WPM descending
                              select compStat).ToListAsync();

            }
            catch (Exception e)
            {
                Log.Information(e.StackTrace);
                Log.Information("No relevant stats found returning empty list");
                return new List<CompetitionStat>();
            }
        }

        public async Task<Tuple<string, string, int>> GetCompStuff(int compId)
        {
            try
            {
                return await (from comp in _context.Competitions
                              where comp.Id == compId
                              select Tuple.Create(comp.TestAuthor, comp.TestString, comp.CategoryId)).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("Competition not found returning null");
                return null;
            }
        }

       
        public async Task<LiveCompetition> GetLiveCompetition(int id)
        {
            try
            {
                LiveCompetition liveCompetition = await(from lC in _context.LiveCompetitions
                                                            where lC.Id == id
                                                            select lC).SingleAsync();
                return liveCompetition;
            }catch(Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("Couldn't get live competition");
                return null;
            }
        }

        public async Task<List<LiveCompetition>> GetLiveCompetitions()
        {
            try
            {
                return await (from lC in _context.LiveCompetitions
                              select lC).ToListAsync();
            } catch(Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("Returning empty list for live competitions");
                return new List<LiveCompetition>();
            }
        }

        public async Task<List<LiveCompetitionTest>> GetLiveCompetitionTestsForCompetition(int compId)
        {
            try
            {
                List<LiveCompetitionTest> liveCompetitionTests = await(from lCT in _context.LiveCompetitionTests
                                                                       where lCT.LiveCompetitionId == compId
                                                                       select lCT).ToListAsync();
                return liveCompetitionTests;
            }catch(Exception e)
            {
                Log.Information("No competitions found returning empty list");
                Log.Information(e.Message);
                return new List<LiveCompetitionTest>();
            }
        }

        public async Task<List<UserQueue>> GetLiveCompetitionUserQueue(int liveCompId)
        {
            try
            {
                List<UserQueue> userQueues = await(from uQ in _context.UserQueues
                                                   where uQ.LiveCompetitionId == liveCompId
                                                   orderby uQ.EnterTime ascending
                                                   select uQ).ToListAsync();
                return userQueues;
            }catch(Exception e)
            {
                Log.Information("No user queues found returning new list");
                Log.Information(e.Message);
                return new List<UserQueue>();
            }
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                return await (from u in _context.Users
                              where u.Id == id
                              select u).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("User Not Found");
                return null;
            }
        }

        public async Task<User> GetUser(string auth0id)
        {
            try
            {
                return await (from u in _context.Users
                              where u.Auth0Id == auth0id
                              select u).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("User not found");
                return null;
            }
        }

        public async Task<CompetitionStat> UpdateCompStat(CompetitionStat c)
        {
            try
            {
                _context.CompetitionStats.Update(c);
                await _context.SaveChangesAsync();
                return c;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error adding competitionstat returning null");
                return null;
            }
        }

        public async Task<List<LiveCompStat>> GetLiveCompStats(int liveCompId)
        {
            try
            {
                List<LiveCompStat> liveCompStats = await(from lCS in _context.LiveCompStats
                                                         where lCS.LiveCompetitionId == liveCompId
                                                         orderby lCS.Wins
                                                         select lCS).ToListAsync();
                return liveCompStats;
            }catch(Exception e)
            {
                Log.Information(e.StackTrace);
                Log.Information("Sending back empty list for Get LCS");
                return new List<LiveCompStat>();
            }
        }
    }
}
