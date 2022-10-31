using MediatR;

namespace RecSys.Api.Areas.Auth.Actions.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, AuthenticateResponse>
    {
        private readonly IMediator _mediator;

        public RegisterUserHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<AuthenticateResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            // var userId = Guid.NewGuid();
            //
            // var user = await _repository.GetUserByUsernameAsync(new GetUserByUsernameCmd(request.Username), cancellationToken);
            // if (user != null)
            //     throw new UserAlreadyExistsException($"Имя пользователя: {request.Username} занято");
            //
            // var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            // var newUser = await _repository.InsertNewUserAsync(new InsertUserCmd(userId, request.Username, hashedPassword), cancellationToken);
            //
            // var newRequest = new AuthenticateCommand(newUser.Username, request.Password, request.IsExtended)
            // {
            //     IpAddress = request.IpAddress
            // };
            //
            // return await _mediator.Send(newRequest, cancellationToken);
            await Task.Delay(1);
            return new AuthenticateResponse("1", "1");
        }
    }
}
