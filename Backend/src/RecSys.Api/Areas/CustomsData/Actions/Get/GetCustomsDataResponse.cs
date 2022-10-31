using RecSys.Api.CommonDtos;

namespace RecSys.Api.Areas.CustomsData.Actions.Get;

public record GetCustomsDataResponse(CustomsElement[] CustomsElements, PaginationResponse PaginationResponse);
