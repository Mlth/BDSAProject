using Microsoft.AspNetCore.Mvc;
using LibGit2Sharp;
using GitInsight;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AnalysisController : ControllerBase
{
    [HttpGet("{command}")]
    public List<string> Get(string command)
    {
        var repositoryPath = "/Users/anton/Desktop/BDSAProject/";

        WebProgram webProgram = new WebProgram();
        var list = webProgram.GetAnalysisList(repositoryPath, command); 

        return list;
    }
}
