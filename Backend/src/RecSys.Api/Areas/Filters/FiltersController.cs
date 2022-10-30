using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RecSys.Api.Areas.Filters;

[ApiController]
[Route("v1/filters")]
public class FiltersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CancellationToken _cancellationToken;

    public FiltersController(IMediator mediator)
    {
        _mediator = mediator;
        _cancellationToken = HttpContext.RequestAborted;
    }
}
