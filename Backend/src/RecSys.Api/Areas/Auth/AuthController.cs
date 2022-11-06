using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecSys.Api.Areas.Auth.Actions;
using RecSys.Api.Areas.Auth.Actions.Authenticate;
using RecSys.Api.Areas.Auth.Actions.Register;
using RecSys.Api.Areas.Users.Dtos;
using RecSys.Platform.Data.Providers;
using RecSys.Platform.Dtos;
using RecSys.Platform.Exceptions;

namespace RecSys.Api.Areas.Auth;

[ApiController]
[Route("v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly IDbConnectionsProvider _dbConnectionsProvider;

    public AuthController(IMediator mediator, IConfiguration configuration, IDbConnectionsProvider dbConnectionsProvider)
    {
        _mediator = mediator;
        _configuration = configuration;
        _dbConnectionsProvider = dbConnectionsProvider;
    }

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="request">Логин и пароль.</param>
    /// <returns>JWT и Refresh токен.</returns>
    [HttpPost("authenticate")]
    [Authorize]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(AuthenticateResponse))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> Authenticate([FromQuery] AuthenticateUserRequest request)
    {
        var connection = _dbConnectionsProvider.GetConnection();
        var query = @"select * from users where username = :Username";
        var qq = await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = request.Login });
        if (qq is null)
            throw new ExceptionWithCode(404, " ");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, qq.Password))
            throw new ExceptionWithCode(403, " ");
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", qq.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, request.Login),
                new Claim(JwtRegisteredClaimNames.Email, request.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return Ok(new AuthenticateResponse(jwtToken, jwtToken));
    }

    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="request">Информация для регистрации.</param>
    /// <returns>JWT и Refresh токены.</returns>
    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(AuthenticateResponse))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> Register([FromQuery] RegisterUserRequest request)
    {
        // .WithColumn("id").AsInt64().Identity().PrimaryKey()
        //     .WithColumn("username").AsString().Unique()
        //     .WithColumn("password").AsString()
        //     .WithColumn("email").AsString()
        //     .WithColumn("profile_pic_url").AsString().Nullable()
        //     .WithColumn("first_name").AsString()
        //     .WithColumn("second_name").AsString()
        //     .WithColumn("middle_name").AsString().Nullable()
        //     .WithColumn("role").AsString().ForeignKey("roles", "role");
        var connection = _dbConnectionsProvider.GetConnection();
        var query = @"select * from users where username = :Username";
        var qq = await connection.QueryFirstOrDefaultAsync<User>(query, new { request.Username });
        if (qq is not null)
            throw new ExceptionWithCode(400, " ");
        var insertQuery =
            @"insert into users (username, password, email, profile_pic_url, first_name, second_name, middle_name, role)
values (:Username, :Password, :Email, :ProfilePicUrl, :FirstName, :SecondName, :MiddleName, :Role)";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        await connection.ExecuteAsync(
            insertQuery,
            new
            {
                request.Username,
                Password = hashedPassword,
                request.Email,
                request.ProfilePicUrl,
                request.SecondName,
                request.FirstName,
                request.MiddleName,
                Role = "user",
            });
        qq = await connection.QueryFirstOrDefaultAsync<User>(query, new { request.Username });
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", qq.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Email, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return Ok(new AuthenticateResponse(jwtToken, jwtToken));
    }

    private async Task<IActionResult> MediateOkAsync(object request)
    {
        var response = await _mediator.Send(request, HttpContext.RequestAborted);
        return Ok(response);
    }
}
