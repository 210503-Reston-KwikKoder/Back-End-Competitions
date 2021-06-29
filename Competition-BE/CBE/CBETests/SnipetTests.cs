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
                //ISnippets Coverage
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

                
                /*[Fact]
                public async Task RandomCodeShouldNotBeNull()
                {
                        ISnippets _snipetService = new Snippets(s);
                        TestMaterial test1 = await _snipetService.GetCodeSnippet(1);
                        Assert.NotNull(test1);
               
                }
                [Fact]
                public async Task RandomCodeShouldReturnRandom()
                {
                
                }*/

        }
}