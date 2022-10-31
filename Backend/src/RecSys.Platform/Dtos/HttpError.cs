namespace RecSys.Platform.Dtos;

public record HttpError(string ErrorMessage, string? StackTrace = null);
