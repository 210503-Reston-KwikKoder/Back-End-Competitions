using CBEBL;
using CBEModels;
using CBERest.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CBERest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveCompetitionController : ControllerBase
    {
        private readonly ISnippets _snippetsService;
        private readonly ICompBL _compBL;
        private readonly ICategoryBL _categoryBL;
        private readonly IUserBL _userBL;
        private readonly ApiSettings _ApiSettings;

        public LiveCompetitionController(ICompBL compBL, ICategoryBL catBL, IUserBL uBL, IOptions<ApiSettings> settings, ISnippets snippets)
        {
            _compBL = compBL;
            _categoryBL = catBL;
            _userBL = uBL;
            _ApiSettings = settings.Value;
            _snippetsService = snippets;
        }
        // GET: api/<LiveCompetitionController>
        [HttpGet]
        public async Task<IEnumerable<LiveCompOutput>> Get()
        {
            List<LiveCompOutput> liveCompOutputs = new List<LiveCompOutput>();
            List<LiveCompetition> liveCompetitions = await _compBL.GetLiveCompetitions();
            foreach (LiveCompetition liveCompetition in liveCompetitions)
            {
                LiveCompOutput liveCompOutput = new LiveCompOutput();
                liveCompOutput.Id = liveCompetition.Id;
                liveCompOutput.Name = liveCompetition.Name;
                liveCompOutputs.Add(liveCompOutput);
            }
            return liveCompOutputs;
        }

        // GET api/<LiveCompetitionController>/5
        [HttpGet("{id}", Name = "GetComp")]
        public async Task<IEnumerable<LiveCompTestOutput>> Get(int id)
        {
            List<LiveCompetitionTest> liveCompetitionTests = await _compBL.GetLiveCompetitionTestsForCompetition(id);
            List<LiveCompTestOutput> liveCompTestOutputs = new List<LiveCompTestOutput>();
            foreach (LiveCompetitionTest liveCompetitionTest in liveCompetitionTests)
            {
                LiveCompTestOutput liveCompTestOutput = new LiveCompTestOutput();
                liveCompTestOutput.Category = (await _categoryBL.GetCategoryById(liveCompetitionTest.CategoryId)).Name;
                liveCompTestOutput.CompId = liveCompetitionTest.LiveCompetitionId;
                liveCompTestOutput.TestAuthor = liveCompetitionTest.TestAuthor;
                liveCompTestOutput.TestString = liveCompetitionTest.TestString;
                liveCompTestOutputs.Add(liveCompTestOutput);
            }
            return liveCompTestOutputs;
        }

        // POST api/<LiveCompetitionController>

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostAsync(LiveCompInput liveCompInput)
        {
            LiveCompetition liveCompetition = new LiveCompetition();
            liveCompetition.Name = liveCompInput.Name;
            int compId = await _compBL.AddLiveCompetition(liveCompetition);
            if (compId == -1) return BadRequest();
            return CreatedAtRoute(
                                       routeName: "GetComp",
                                       routeValues: new { id = compId },
                                       value: compId);
        }
        [HttpPut("nexttest")]
        [Authorize]
        public async Task<ActionResult> PutAsync(LiveCompTestInput liveCompTestInput)
        {
            try
            {
                TestMaterial newTest = await _snippetsService.GetCodeSnippet(liveCompTestInput.category);
                LiveCompetitionTest liveCompetitionTest = new LiveCompetitionTest();
                liveCompetitionTest.TestAuthor = newTest.author;
                liveCompetitionTest.TestString = newTest.content;
                if (await _categoryBL.GetCategory(liveCompTestInput.category) == null)
                {
                    Category category = new Category();
                    category.Name = liveCompTestInput.category;
                    await _categoryBL.AddCategory(category);
                }
                Category category1 = await _categoryBL.GetCategory(liveCompTestInput.category);
                liveCompetitionTest.CategoryId = category1.Id;
                liveCompetitionTest.LiveCompetitionId = liveCompTestInput.compId;
                await _compBL.AddLiveCompetitionTest(liveCompetitionTest);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("Unexpected error returning 400");
                return NotFound();
            }
        }
    }
}