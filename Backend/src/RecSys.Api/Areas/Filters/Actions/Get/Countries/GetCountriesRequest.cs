using MediatR;

namespace RecSys.Api.Areas.Filters.Actions.Get.Countries;

public record GetCountriesRequest : IRequest<GetCountriesResponse>;
