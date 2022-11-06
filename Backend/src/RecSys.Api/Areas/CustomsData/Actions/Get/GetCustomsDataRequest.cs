using MediatR;
using RecSys.Api.CommonDtos;

namespace RecSys.Api.Areas.CustomsData.Actions.Get;

public record GetCustomsDataRequest(Filter? Filter, Pagination Pagination) : IRequest<GetCustomsDataResponse>;
