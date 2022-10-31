using System.Text.Json.Serialization;
using MediatR;

namespace RecSys.Api.Areas.Auth.Actions.Register
{
    public record RegisterUserRequest
        (string Username, string Password, bool IsExtended = false) : IRequest<AuthenticateResponse>
    {
        [JsonIgnore]
        public string IpAddress { get; set; } = "0.0.0.0";
    }
}
