using Microsoft.AspNetCore.Mvc;

using parse_service.Managers;
using parse_service.Repositories;
namespace parse_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ParseController : ControllerBase
{
    private readonly ILogger<dynamic> logger;
    private readonly IOdbcRepository odbcRepository;
    private readonly IParseManager parseManager;

    public ParseController(ILogger<dynamic> logger, IParseManager parseManager, IOdbcRepository odbcRepository)
    {
        this.logger = logger;
        this.parseManager = parseManager;
        this.odbcRepository = odbcRepository;
    }

    [HttpPost(Name = "xls")]
    [Route("xls")]
    public ParseResponse Post(IFormFile file, [FromForm] int headerIndex)
    {
        try
        {
            var data = parseManager.FromXls(file, new ParseOptions { headerIndex = headerIndex });
            return new ParseResponse(data, "json");
        }
        catch (Exception error)
        {
            this.logger.LogError("Error on ParseController.Post", file, headerIndex, error);
            var msg = $"Failed to parse {file.FileName} with header at index {headerIndex}";
            throw error;
        }
    }

    [HttpPost(Name = "mdb")]
    [Route("mdb")]
    public ParseResponse Post(IFormFile file, [FromForm] string tableName)
    {
        try
        {
            var data = this.odbcRepository.Get(file, tableName);
            return new ParseResponse(data, "json");
        }
        catch (Exception error)
        {
            this.logger.LogError("Error on ParseController.Post", file, error);
            var msg = $"Failed to parse mdb {file.FileName}";
            throw error;
        }
    }
}

