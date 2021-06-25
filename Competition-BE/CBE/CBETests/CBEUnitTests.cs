using System;
using Xunit;
using CBEDL;
using CBEBL;
using CBERest;
using CBEModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Serilog;
using Moq;
using System.Collections.Generic;
using CBERest.Controllers;
using Microsoft.Extensions.Options;
using CBERest.DTO;
using Microsoft.AspNetCore.Mvc;
using GACDRest.Controllers;

namespace CBETests
{
    public class CBEUnitTests
    {
        private readonly DbContextOptions<CBEDbContext> options;
        public CBEUnitTests()
        {
            options = new DbContextOptionsBuilder<CBEDbContext>().UseSqlite("Filename=Test.db;").Options;
            Seed();
        }
        /// <summary>
        /// Method to make sure AddUser adds a user to the db
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddUserShouldAddUserAsync()
        {
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
        /// Asserts that competiton is created and the id is not -1 (error)
        /// </summary>
        /// <returns>True if comp Id is valid, false otherwise</returns>
        [Fact]
        public async Task CompetitionShouldBeCreated()
        {
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
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
            using (var context = new CBEDbContext(options))
            {
                ICompBL compBL = new CompBL(context);
                int expected = 0;
                int actual = (await compBL.GetAllCompetitions()).Count;
                Assert.Equal(expected, actual);
            }
        }

        /// <summary>
        /// Making sure getting a bad competition is null
        /// </summary>
        /// <returns>True on success</returns>
        [Fact]
        public async Task GettingBadCompetitionByIdShouldBeNull()
        {
            using (var context = new CBEDbContext(options))
            {
                ICompBL compBL = new CompBL(context);
                Assert.Null(await compBL.GetCompetition(1));
            }
        }
        [Fact]
        public async Task AddingToWhiteListShouldWork()
        {
            using (var context = new CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp, "testauthor", true);
                bool actual = await compBL.WhiteListUser(compId, 2);
                bool expected = true;
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public async Task CheckTheListShouldReturnTrue()
        {
            using (var context = new CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp, "testauthor", true);
                await compBL.WhiteListUser(compId, 2);
                bool actual = await compBL.CheckTheList(compId, 2);
                bool expected = true;
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public async Task GetWhiteListShouldReturnCorrectNumbe()
        {
            using (var context = new CBEDbContext(options))
            {
                Competition c = new Competition();
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                ICompBL compBL = new CompBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Category category1 = await categoryBL.GetCategory(1);
                string testForComp = "Console.WriteLine('Hello World');";
                int compId = await compBL.AddCompetition(DateTime.Now, DateTime.Now, category1.Id, "name", 1, testForComp, "testauthor", true);
                await compBL.WhiteListUser(compId, 2);
                int actual = (await compBL.GetWhiteList(compId)).Count;
                int expected = 1;
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public async Task CompetitionControllerShouldReturnListOfCompetitionObject()
        {
            var mockCompBL = new Mock<ICompBL>();
            mockCompBL.Setup(x => x.GetAllCompetitions()).ReturnsAsync(
                new List<Competition>
                {
                    new Competition(),
                    new Competition()
                }
                );
            var mockCatBL = new Mock<ICategoryBL>();
            var mockUserBL = new Mock<IUserBL>();
            var settings = Options.Create(new ApiSettings());

            var controller = new CompetitionController(mockCompBL.Object, mockCatBL.Object, mockUserBL.Object, settings);
            var result = await controller.GetAsync();
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<CompetitionObject>>>(result);
        }

        [Fact]
        public async Task CompetitionControllerShouldReturnListOfUsers()
        {
            var mockCompBL = new Mock<ICompBL>();
            var mockCatBL = new Mock<ICategoryBL>();
            var mockUserBL = new Mock<IUserBL>();
            mockUserBL.Setup(x => x.GetUsers()).ReturnsAsync(
                new List<User>
                {
                    new User(),
                    new User()
                }
                );
            var settings = Options.Create(new ApiSettings());

            var controller = new CompetitionController(mockCompBL.Object, mockCatBL.Object, mockUserBL.Object, settings);
            var result = await controller.GetAllUsers();
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<UserNameModel>>>(result);
        }

        [Fact]
        public async Task CompetitionControllerShouldReturnListOfCompStatOutput()
        {
            var mockCompBL = new Mock<ICompBL>();
            mockCompBL.Setup(x => x.GetCompetitionStats(1)).ReturnsAsync(
                new List<CompetitionStat>
                {
                    new CompetitionStat(),
                    new CompetitionStat()
                }
                );
            var mockCatBL = new Mock<ICategoryBL>();
            var mockUserBL = new Mock<IUserBL>();
            var settings = Options.Create(new ApiSettings());

            var controller = new CompetitionController(mockCompBL.Object, mockCatBL.Object, mockUserBL.Object, settings);
            var result = await controller.GetAsync(1);
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<CompStatOutput>>>(result);
        }

        [Fact]
        public async Task CompetitionControllerShouldReturnListOfWhitelistUsers()
        {
            var mockCompBL = new Mock<ICompBL>();
            var mockCatBL = new Mock<ICategoryBL>();
            var mockUserBL = new Mock<IUserBL>();
            mockUserBL.Setup(x => x.GetUser(1)).ReturnsAsync(new User());
            var settings = Options.Create(new ApiSettings());

            var controller = new CompetitionController(mockCompBL.Object, mockCatBL.Object, mockUserBL.Object, settings);
            var result = await controller.GetWhiteList(1);
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<UserNameModel>>>(result);
        }

        [Fact]
        public async Task CompetitionTestControllerShouldReturnCompetitionContent()
        {
            var mockCompBL = new Mock<ICompBL>();
            mockCompBL.Setup(x => x.GetCompStuff(1)).ReturnsAsync(Tuple.Create("author", "string", 1));
            var mockCatBL = new Mock<ICategoryBL>();
            var mockUserBL = new Mock<IUserBL>();

            var controller = new CompetitonTestsController(mockUserBL.Object, mockCatBL.Object, mockCompBL.Object);
            var result = await controller.Get(1);
            Assert.NotNull(result);
            Assert.IsType<ActionResult<CompetitionContent>>(result);
        }




        private void Seed()
        {
            using (var context = new CBEDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

    } 
}
