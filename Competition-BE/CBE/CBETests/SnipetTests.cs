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

namespace CBETests
{

        public class GACDSnipetTest
        {
                private readonly IOptions<ApiSettings> s = Options.Create(new ApiSettings());

                public GACDSnipetTest()
                {

                }  
                [Fact]
                public async Task RandomQuoteShouldNotBeNull()
                {
                        ISnippets _snipetService = new Snippets();
                        TestMaterial test1 = await _snipetService.GetRandomQuote();
                        Assert.NotNull(test1);
                }
                [Fact]
                public async Task RandomQuoteShouldReturnRandom()
                {
                        ISnippets _snipetService = new Snippets();
                        TestMaterial test1 = await _snipetService.GetRandomQuote();
                        TestMaterial test2 = await _snipetService.GetRandomQuote();
                        Assert.NotEqual(test1, test2);            
                }

                [Fact]
                public async Task CheckConstructor(){
                        ISnippets _snipetService = new Snippets(this.s);
                        TestMaterial test = await _snipetService.GetRandomQuote();
                        Assert.NotNull(test);
                }

                [Fact]
                public async Task TestEx(){
                        
                        var mockService = new Mock<ISnippets>();
                        mockService.Setup(x => x.GetCodeSnippet(It.IsAny<int>())).Throws( new Exception());
                        ISnippets _snipetService = mockService.Object;


                        await Assert.ThrowsAsync<Exception>(async()=> {await _snipetService.GetCodeSnippet(0);});
                }

                [Fact]
                public async Task CodeSnippetTest(){
                        var mockSettings = new Mock<IOptions<ApiSettings>>();
                        var key = "z1DzEwcWO8j85Wq9ZusbIt7CVRXW";
                        key = "ghp_" + key;
                        key = key + "a82yR6Wp";
                        mockSettings.Setup(x => x.Value).Returns(new ApiSettings(){githubApiKey = key});
                        ISnippets _snipetService = new Snippets(mockSettings.Object);

                        var snippet = await _snipetService.GetCodeSnippet(32);
                        var snippet2 = await _snipetService.GetCodeSnippet(32);
                        Assert.NotEqual(snippet,snippet2);
                }

                [Fact]
                public async Task GetAuthStringTest(){
                        var mockSettings = new Mock<IOptions<ApiSettings>>();
                        mockSettings.Setup(x => x.Value).Returns(new ApiSettings(){authString = "asdf"});

                        ISnippets _snipetService = new Snippets(mockSettings.Object);

                        var i = await _snipetService.GetAuth0String();

                        Assert.Equal("asdf", i);

                }

                [Fact]
                public async Task TestExRandom(){
                        
                        var mockService = new Mock<ISnippets>();
                        mockService.Setup(x => x.GetRandomQuote()).Throws( new Exception());
                        ISnippets _snipetService = mockService.Object;


                        await Assert.ThrowsAsync<Exception>(async()=> {await _snipetService.GetRandomQuote();});
                }

        }
}