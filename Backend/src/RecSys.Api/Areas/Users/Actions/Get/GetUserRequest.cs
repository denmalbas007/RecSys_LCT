using MediatR;

namespace RecSys.Api.Areas.Users.Actions.Get;

public record GetUserRequest(long UserId) : IRequest<GetUserResponse>;
