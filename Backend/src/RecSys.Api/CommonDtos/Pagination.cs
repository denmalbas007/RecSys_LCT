using System.ComponentModel.DataAnnotations;

namespace RecSys.Api.CommonDtos;

public record Pagination([Range(0, long.MaxValue)] long Offset, [Range(1, 500)] long Count);
