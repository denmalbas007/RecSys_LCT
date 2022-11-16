using Dapper;
using Microsoft.AspNetCore.Mvc;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.Areas.Files;

[ApiController]
[Route("v1/files")]
public class FilesController : ControllerBase
{
    private readonly IDbConnectionsProvider _connectionsProvider;

    public FilesController(IDbConnectionsProvider connectionsProvider)
        => _connectionsProvider = connectionsProvider;

    [HttpGet("{file}")]
    public async Task<IActionResult> GetFile(string file)
    {
        var connection = _connectionsProvider.GetConnection();
        var result = await connection.QueryFirstOrDefaultAsync<string>(@"select bytes from storage where id = :Id::uuid", new { Id = file });
        var type = await connection.QueryFirstOrDefaultAsync<string>(@"select type from storage where id = :Id::uuid", new { Id = file });
        var bytes = Convert.FromBase64String(result);
        return File(bytes, $"{type}/application", fileDownloadName: $"{file}.{type}");
    }
}
