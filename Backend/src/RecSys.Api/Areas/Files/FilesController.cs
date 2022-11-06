using Microsoft.AspNetCore.Mvc;

namespace RecSys.Api.Areas.Files;

[ApiController]
[Route("v1/files")]
public class FilesController : ControllerBase
{
    [HttpGet("{file}")]
    public async Task<IActionResult> GetFile(string file)
    {
        await Task.Delay(1);
        return File(new byte[7000], "pdf/application", fileDownloadName: $"{file}.pdf");
    }
}
