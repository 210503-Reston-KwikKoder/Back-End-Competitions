using System.Threading.Tasks;
using CBEModels;
using Octokit;

namespace CBEBL
{
    public interface ISnippets
    {
        Task<TestMaterial> GetRandomQuote();
        Task<TestMaterial> GetCodeSnippet(int id);
        Task<string> GetAuth0String();
    }
}