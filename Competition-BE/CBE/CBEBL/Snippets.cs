using System.IO;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CBEModels;
using Microsoft.Extensions.Options;
using Octokit;
using Serilog;

namespace CBEBL
{
    public class Snippets : ISnippets
    {
        private readonly ApiSettings _ApiSettings;
        public Snippets(){
            
        }
        public Snippets(IOptions<ApiSettings> settings){
            _ApiSettings = settings.Value;
        }
        
        public async Task<TestMaterial> GetRandomQuote()
        {
            try{
                Log.Debug("Getting Random Quote From API");

                string endpoint = "http://api.quotable.io/random";

                var response = DoHttpRequest(endpoint);

                //convert response string to TestMaterial
                
                TestMaterial quote = await JsonSerializer.DeserializeAsync<TestMaterial>(await response);
                Log.Debug("Content: {0}", quote.content);           
                return quote;
            }catch(Exception ex){
                Log.Error("Error Getting Random Quote {0}\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return null;
            }       
        }
        public async Task<string> GetAuth0String()
        {
            return _ApiSettings.authString;
        }
        public async Task<TestMaterial> GetCodeSnippet(int id)
        {
            try{
                Log.Debug("Getting code snipped for language: {0}",id);
                var client = new GitHubClient(new ProductHeaderValue("Not-sure-why-I-need-this"));
                var tokenAuth = new Credentials(_ApiSettings.githubApiKey); // NOTE: not real token
                client.Credentials = tokenAuth;

                var request = new SearchCodeRequest{
                    Language = (Language)id
                };

                SearchCodeResult searchResult= await client.Search.SearchCode(request);
                
                String htmlUrl = searchResult.Items[0].HtmlUrl;
                //convert html url to raw.githubusercontent
                htmlUrl = htmlUrl.Replace("/blob/", "/");
                String author = searchResult.Items[0].Repository.FullName;

                htmlUrl = htmlUrl.Replace("https://github.com/", "https://raw.githubusercontent.com/");
                
                //collect text from site
                var rawSnippet = await DoHttpRequest(htmlUrl);
                
                //parse rawSnippet
                    //remove Comments
                    //remove usings
                    //remove trailing empty lines
                    //remove trainling spaces
                    //Make sure to return clean code

                //Check Length?
                StreamReader reader = new StreamReader(rawSnippet);
                string parsedSnippet = await reader.ReadToEndAsync();
                TestMaterial testMaterial = new TestMaterial(parsedSnippet, author, parsedSnippet.Length );
                testMaterial.categoryId = id;
                return testMaterial;
            }catch(Exception ex){
                Log.Error("Error Getting Code Snippet {0}\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return null;
            }
           
        }

        private async Task<Stream> DoHttpRequest(string uri){
            try{
                var httpClient = new HttpClient();
                var URI = new Uri(uri);
                HttpResponseMessage response = await httpClient.GetAsync(URI);
                if (response.IsSuccessStatusCode)
                {
                    Log.Debug( $"Retrieved Http Response for uri: {uri}");
                    return await response.Content.ReadAsStreamAsync();
                   
                }else{
                    Log.Warning($"Http Requst for uri: {uri} Failed. Status code:{response.StatusCode}");
                    return null;
                }
            }catch(Exception ex){
                Log.Error("Error doing Http Request {0}\nStack Trace: {1}",ex.Message,ex.StackTrace);
                return null;
            }
        }
    }
}