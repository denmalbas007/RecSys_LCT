namespace RecSys.Api.Areas.Users.Dtos;

public class User
{
    public long Id { get; init; }

    public string Username { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string? MiddleName { get; init; }

    public string LastName { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string? ProfilePicUrl { get; init; }

    public long[] ReportIds { get; init; } = Array.Empty<long>();

    public long[] LayoutIds { get; init; } = Array.Empty<long>();
}
