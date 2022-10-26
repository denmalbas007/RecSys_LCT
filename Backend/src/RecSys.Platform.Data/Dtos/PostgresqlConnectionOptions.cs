using System.ComponentModel.DataAnnotations;

namespace RecSys.Platform.Data.Dtos;

public class PostgresqlConnectionOptions
{
    [Required]
    public string Username { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;

    [Required]
    public string Host { get; init; } = null!;

    public int Port { get; init; } = 5432;

    [Required]
    public string Database { get; init; } = null!;

    public bool Pooling { get; init; } = true;

    public int MinPoolSize { get; init; }

    public int MaxPoolSize { get; init; } = 100;

    public int ConnectionLifetime { get; init; } = 30;
}
