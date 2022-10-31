namespace RecSys.Api.Areas.Auth.Actions;

public record AuthenticateResponse(string JwtToken, string RefreshToken);
