using System.Text.Json.Serialization;

namespace RecSys.Api.Areas.Users.Dtos;

public class User
{
    public long Id { get; init; }

    public string Username { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string? MiddleName { get; init; }

    public string SecondName { get; init; } = null!;

    public string Email { get; init; } = null!;

    [JsonIgnore]
    public string? Password { get; init; }

    public string? ProfilePicUrl { get; init; }

    public long[] ReportIds { get; set; } = Array.Empty<long>();

    public long[] LayoutIds { get; set; } = Array.Empty<long>();
}
