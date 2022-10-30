using MediatR;

namespace RecSys.Api.Areas.Auth.Actions.Authenticate;

public class AuthenticateUserRequest : IRequest<AuthenticateResponse>
{
}
