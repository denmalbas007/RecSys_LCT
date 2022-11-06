using RecSys.Api.CommonDtos;

namespace RecSys.Api.Areas.Reports.Actions.Create;

public record CreateReportRequest(string Name, Filter Filter);
