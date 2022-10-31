using RecSys.Api.CommonDtos;

namespace RecSys.Api.Areas.Layouts.Dtos;

public record Layout(long Id, string Name, Filter Filter, DateTime LastUpdatedAt, DateTime CreatedAt);
