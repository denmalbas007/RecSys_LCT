using MediatR;

namespace RecSys.Api.Areas.Auth.Actions.Register
{
    public record RegisterUserRequest
        (string Username, string Password, string Email, string? ProfilePicUrl, string FirstName, string SecondName, string? MiddleName) : IRequest<AuthenticateResponse>;
}
