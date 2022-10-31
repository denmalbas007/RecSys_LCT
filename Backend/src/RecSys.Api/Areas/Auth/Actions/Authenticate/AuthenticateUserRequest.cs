using MediatR;

namespace RecSys.Api.Areas.Auth.Actions.Authenticate;

public record AuthenticateUserRequest(string Login, string Password) : IRequest<AuthenticateResponse>;
