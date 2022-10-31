using MediatR;
using RecSys.Api.CommonDtos;

namespace RecSys.Api.Areas.Layouts.Actions.Create;

public record CreateLayoutRequest(Filter Filter, string? Name = null) : IRequest<CreateLayoutResponse>;
