using System;
using Xunit;
using CBEDL;
using CBEBL;
using CBERest;
using CBEModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Serilog;
namespace CBETests
{
    public class CBEUnitTests
    {
        private readonly DbContextOptions<CBEDbContext> options;
        public CBEUnitTests()
        {
            options = new DbContextOptionsBuilder<CBEDbContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }
        /// <summary>
        /// Method to make sure AddUser adds a user to the db
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddUserShouldAddUserAsync()
        {
            using(var context = new CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                int userCount = (await userBL.GetUsers()).Count;
                int expected = 1;
                Assert.Equal(expected, userCount);
            }
        }
        /// <summary>
        /// Makes sure that Categories can be added
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task AddCatShouldAddCatAsync()
        {
            using(var context = new CBEDbContext(options))
            {
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Category category1 = new Category();
                category1.Name = 2;
                await categoryBL.AddCategory(category1);
                Category category2 = new Category();
                category2.Name = 3;
                await categoryBL.AddCategory(category2);
                int catCount = (await categoryBL.GetAllCategories()).Count;
                int expected = 3;
                Assert.Equal(expected, catCount);
            }
        }
        /// <summary>
        /// Makes sure UserStats updates and doesn't fail
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
       /* [Fact]
        public async Task UserStatShouldAddUserStatAsync()
        {
            using(var context = new CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "testid";
                user = await userBL.AddUser(user);
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                category = await categoryBL.AddCategory(category);
                IUserStatBL userStatBL = new UserStatBL(context);
                TypeTest typeTest = new TypeTest();
                typeTest.Date = DateTime.Now;
                typeTest.NumberOfErrors = 1;
                typeTest.NumberOfWords = 3;
                typeTest.WPM = 30;
                typeTest.TimeTaken = 5;
                UserStat ust = await userStatBL.AddTestUpdateStat(1, 1, typeTest);
                Assert.NotNull(ust);
            }
        }*/
        /// <summary>
        /// Makes sure that there aren't any critical failures in Leaderboard / Stat methods
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
       /* [Fact]
        public async Task OverallLeaderBoardShouldReturnAnyNumberofUsers()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Category category1 = new Category();
                category1.Name = 2;
                await categoryBL.AddCategory(category1);
                Category category2 = new Category();
                category2.Name = 3;
                await categoryBL.AddCategory(category2);
                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100000, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                await userStatBL.AddTestUpdateStat(1, 2, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(2, 50, 100000, 100,DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 2, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(3, 50, 100000, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 3, testToBeInserted);
                bool expected = true;
                bool actual = (await userStatBL.GetOverallBestUsers()).Count > 0;
                Assert.Equal(expected, actual);
            }
        }*/
        /// <summary>
        /// Makes sure there aren't any critical failures in Category Leaderboard / Stat methods
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
       /* [Fact]
        public async Task CategoryLeaderBoardShouldReturnAnyNumberofUsers()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);

                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                TypeTest testToBeInserted1 = await userStatBL.SaveTypeTest(2, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 1, testToBeInserted1);
                TypeTest testToBeInserted2 = await userStatBL.SaveTypeTest(3, 50, 100, 100,  DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 1, testToBeInserted2);
                bool expected = true;
                bool actual = (await userStatBL.GetBestUsersForCategory(1)).Count > 0;
                Assert.Equal(expected, actual);
            }
        }*/
        /// <summary>
        /// Makes sure all added users are returned correctly in overall leaderboard
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        /*[Fact]
        public async Task OverallLeaderBoardShouldReturnCorrectNumberofUsers()
        {
            using(var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Category category1 = new Category();
                category1.Name = 2;
                await categoryBL.AddCategory(category1);
                Category category2 = new Category();
                category2.Name = 3;
                await categoryBL.AddCategory(category2);
                User user= new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(2, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 2, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(3, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 3, testToBeInserted);
                int expected = 3;
                int actual = (await userStatBL.GetOverallBestUsers()).Count;
                Assert.Equal(expected, actual);
            }
        }*/
        /// <summary>
        /// Makes sure all users are returned correctly for a given category
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
       /* [Fact]
        public async Task CategoryLeaderBoardShouldReturnCorrectNumberofUsers()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                
                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                TypeTest testToBeInserted1 = await userStatBL.SaveTypeTest(2, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 1, testToBeInserted1);
                TypeTest testToBeInserted2 = await userStatBL.SaveTypeTest(3, 50, 100, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 1, testToBeInserted2);
                int expected = 3;
                int actual = (await userStatBL.GetBestUsersForCategory(1)).Count;
                Assert.Equal(expected, actual);
            }
        }*/
        /// <summary>
        /// Makes sure Average WPM is actual average
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
       /* [Fact]
        public async Task AverageWPMShouldBeAverage()
        {
            using (var context = new  CBEDbContext(options))
            {
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Double avgExpected;
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, 100, DateTime.Now);
                avgExpected = testToBeInserted.WPM/2;
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                TypeTest testToBeInserted1 = await userStatBL.SaveTypeTest(1, 50, 100, 100, DateTime.Now);
                avgExpected += testToBeInserted1.WPM/2;
                Double actual = (await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted)).AverageWPM;
                Assert.Equal(avgExpected, actual);
            }
        }*/
        /// <summary>
        /// Asserts that competiton is created and the id is not -1 (error)
        /// </summary>
        /// <returns>True if comp Id is valid, false otherwise</returns>
        [Fact]
        public async Task CompetitionShouldBeCreated()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                //IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int actual = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp);
                int notExpected = -1;
                Assert.NotEqual(notExpected, actual);
            }
        }
        /// <summary>
        /// Makes sure a competition can be created and the string can be accessed
        /// </summary>
        /// <returns>True if hello world found, false otherwise</returns>
        [Fact]
        public async Task CompetitionStringShouldBeAccessed()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                //IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp);
                string actual = await compBL.GetCompString(compId);
                Assert.Equal(testForComp, actual);
            }
        }

        /// <summary>
        /// Making sure competition adds a single entry without error
        /// </summary>
        /// <returns>True on success, false on fail</returns>
        [Fact]
        public async Task CompetitionShouldAddEntry()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
               // IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp, "testauthor", false);
                CompetitionStat competitionStat = new CompetitionStat();
                competitionStat.WPM = 50;
                competitionStat.UserId = 1;
                competitionStat.CompetitionId = compId;
                int actual = await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                int expected = 1;
                Assert.Equal(expected, actual);
            }
        }

        /// <summary>
        /// Makes sure competition updates rank (last person should be second)
        /// </summary>
        /// <returns>True on success/False on fail</returns>
        [Fact]
        public async Task CompetitionShouldUpdateRank()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                //IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                user = new User();
                user.Auth0Id = "test1";
                await userBL.AddUser(user);
                user = new User();
                user.Auth0Id = "test2";
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp);
                CompetitionStat competitionStat = new CompetitionStat();
                competitionStat.WPM = 50;
                competitionStat.UserId = 1;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                competitionStat = new CompetitionStat();
                competitionStat.WPM = 30;
                competitionStat.UserId = 2;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                competitionStat = new CompetitionStat();
                competitionStat.WPM = 40;
                competitionStat.UserId = 3;
                competitionStat.CompetitionId = compId;
                int actual = await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                int expected = 2;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Checks the GetCompetitionStats method to make sure it correctly returns 3 people
        /// </summary>
        /// <returns>True on success/ False on fail</returns>
        [Fact]
        public async Task CompetitionStatsShouldGetCompStats()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
               // IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                user = new User();
                user.Auth0Id = "test1";
                await userBL.AddUser(user);
                user = new User();
                user.Auth0Id = "test2";
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp);
                CompetitionStat competitionStat = new CompetitionStat();
                competitionStat.WPM = 50;
                competitionStat.UserId = 1;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                competitionStat = new CompetitionStat();
                competitionStat.WPM = 30;
                competitionStat.UserId = 2;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                competitionStat = new CompetitionStat();
                competitionStat.WPM = 40;
                competitionStat.UserId = 3;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                int actual = (await compBL.GetCompetitionStats(compId)).Count;
                int expected = 3;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure adding two of the same category returns null
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task AddingCategoryTwiceShouldBeNull()
        {
            using( var context  = new  CBEDbContext(options))
            {
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Assert.Null(await categoryBL.AddCategory(category));
            }        
        }
        /// <summary>
        /// Makes sure adding two of the same user returns null
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task AddingUserTwiceShouldBeNull()
        {
            using (var context = new  CBEDbContext(options)) {
                IUserBL userBL = new UserBL(context);
                User user  = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                Assert.Null(await userBL.AddUser(user));
            }
        }
        /// <summary>
        /// Makes sure we are able to get a user by their id
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GetUserByIdShouldWork()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                string expected = "test";
                string actual = (await userBL.GetUser(1)).Auth0Id;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure that a user not in the database returns null
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GetBadUserIdShouldBeNull()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                Assert.Null(await userBL.GetUser(1));
            }
        }
        /// <summary>
        /// Just makes sure that a bogus comp id will return no competitionstats
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task EmptyCompetitionShouldHaveEmptyStats()
        {
            using (var context = new  CBEDbContext(options))
            {
                int expected = 0;
                ICompBL compBL = new CompBL(context);
                int actual = (await compBL.GetCompetitionStats(1)).Count;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure that we can retrieve the competition stuff from the database
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task CompStuffShouldBeRetrieved()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
               // IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp, "Ada Lovelace", false);
                Tuple<string, string, int> tuple = await compBL.GetCompStuff(compId);
                Assert.Equal(testForComp, tuple.Item2);
            }
        }
        /// <summary>
        /// Makes sure that we are able to get category by id
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GetCategoryByIdShouldWork()
        {
            using (var context = new  CBEDbContext(options))
            {
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 3;
                await categoryBL.AddCategory(category);
                Category category1 = await categoryBL.GetCategoryById(1);
                int expected = 3;
                int actual = category1.Name;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure competition will show that count is one when we add a competition
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GetCompetitionsShouldGetAComp()
        {
            using (var context = new  CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
               // IUserStatBL userStatBL = new UserStatBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp, "testauthor", false);
                int expected = 1;
                int actual = (await compBL.GetAllCompetitions()).Count;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure that the get competitions is empty without adding a competition
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GetCompetitionsOnEmptyIsNewList()
        {
            using (var context = new  CBEDbContext(options))
            {
                ICompBL compBL = new CompBL(context);
                int expected = 0;
                int actual = (await compBL.GetAllCompetitions()).Count;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure the typetest getting method returns a test we add
        /// </summary>
        /// <returns>True on success</returns>
      /*  [Fact]
        public async Task GetTypeTestsShouldGetaTypeTest()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "testid";
                user = await userBL.AddUser(user);
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                category = await categoryBL.AddCategory(category);
                IUserStatBL userStatBL = new UserStatBL(context);
                TypeTest typeTest = new TypeTest();
                typeTest.Date = DateTime.Now;
                typeTest.NumberOfErrors = 1;
                typeTest.NumberOfWords = 3;
                typeTest.WPM = 30;
                typeTest.TimeTaken = 5;
                UserStat ust = await userStatBL.AddTestUpdateStat(1, 1, typeTest);
                int expected = 1;
                int actual = (await userStatBL.GetTypeTestsForUser(1)).Count;
                Assert.Equal(expected,actual);
            }
        }
        /// <summary>
        /// Makes sure a bet is placed
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task UserWithPoinsShouldPlaceBest()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "testid";
                user = await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "testid1";
                user1 = await userBL.AddUser(user1);
                ICategoryBL categoryBL = new CategoryBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                category = await categoryBL.AddCategory(category);
                IUserStatBL userStatBL = new UserStatBL(context);
                TypeTest typeTest = new TypeTest();
                typeTest.Date = DateTime.Now;
                typeTest.NumberOfErrors = 100;
                typeTest.NumberOfWords = 3000;
                typeTest.WPM = 30;
                typeTest.TimeTaken = 500000;
                UserStat ust = await userStatBL.AddTestUpdateStat(1, 1, typeTest);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, 1, "name", 1, testForComp, "author");
                CompetitionStat competitionStat = new CompetitionStat();
                competitionStat.WPM = 30;
                competitionStat.UserId = 2;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                Assert.NotNull(await compBL.PlaceBet("testid", 2, 1, 1));
            }
        }
        /// <summary>
        /// Making sure that user can place a bet and get a payout
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task ClaimingWinSHouldGivePoints()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "testid";
                user = await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "testid1";
                user1 = await userBL.AddUser(user1);
                ICategoryBL categoryBL = new CategoryBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                category = await categoryBL.AddCategory(category);
                IUserStatBL userStatBL = new UserStatBL(context);
                TypeTest typeTest = new TypeTest();
                typeTest.Date = DateTime.Now;
                typeTest.NumberOfErrors = 100;
                typeTest.NumberOfWords = 3000;
                typeTest.WPM = 30;
                typeTest.TimeTaken = 500000;
                UserStat ust = await userStatBL.AddTestUpdateStat(1, 1, typeTest);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, 1, "name", 1, testForComp, "author");
                CompetitionStat competitionStat = new CompetitionStat();
                competitionStat.WPM = 30;
                competitionStat.UserId = 2;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                await compBL.PlaceBet("testid", 2, 1, 1);
                int points = (await userBL.GetUser(1)).Revapoints;
                await compBL.ClaimBets(1);
                bool expected = true;
                bool actual = ((await userBL.GetUser(1)).Revapoints) > points;
                Assert.Equal(expected, actual);

            }
        }
        /// <summary>
        /// Making sure you can place two bets and get even more points
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task ClaimingWinOnDoubleShouldGivePoints()
        {
            using (var context = new  CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "testid";
                user = await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "testid1";
                user1 = await userBL.AddUser(user1);
                ICategoryBL categoryBL = new CategoryBL(context);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                category = await categoryBL.AddCategory(category);
                IUserStatBL userStatBL = new UserStatBL(context);
                TypeTest typeTest = new TypeTest();
                typeTest.Date = DateTime.Now;
                typeTest.NumberOfErrors = 100;
                typeTest.NumberOfWords = 3000;
                typeTest.WPM = 30;
                typeTest.TimeTaken = 500000;
                UserStat ust = await userStatBL.AddTestUpdateStat(1, 1, typeTest);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, 1, "name", 1, testForComp, "author");
                CompetitionStat competitionStat = new CompetitionStat();
                competitionStat.WPM = 30;
                competitionStat.UserId = 2;
                competitionStat.CompetitionId = compId;
                await compBL.InsertCompStatUpdate(competitionStat, 100, 6);
                await compBL.PlaceBet("testid", 2, 1, 1);
                await compBL.PlaceBet("testid", 2, 1, 1);
                int points = (await userBL.GetUser(1)).Revapoints;
                await compBL.ClaimBets(1);
                bool expected = true;
                bool actual = ((await userBL.GetUser(1)).Revapoints) > points;
                Assert.Equal(expected, actual);

            }
        }
        /// <summary>
        /// Makes sure that placing an incorrect bet returns null
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task PlacingBadBetShouldBeNull()
        {
            using (var context = new  CBEDbContext(options))
            {
                ICompBL compBL = new CompBL(context);
                Assert.Null(await compBL.PlaceBet("testid", 2, 1, 1));
            }
        }
        /// <summary>
        /// Making sure getting a bad competition is null
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GettingBadCompetitionByIdShouldBeNull()
        {
            using (var context = new  CBEDbContext(options))
            {
                ICompBL compBL = new CompBL(context);
                Assert.Null(await compBL.GetCompetition(1));
            }
        }*/
        private void Seed()
        {
            using(var context = new  CBEDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
