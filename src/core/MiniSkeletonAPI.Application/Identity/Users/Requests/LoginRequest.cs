using System.Text.Json.Serialization;

namespace MiniSkeletonAPI.Application.Identity.Users.Requests;
public sealed class LoginRequest
{
    [JsonPropertyName("nik")]
    public required string Nik { get; init; }
    [JsonPropertyName("password")]
    public required string Password { get; init; }
    public string? TwoFactorCode { get; init; }
    public string? TwoFactorRecoveryCode { get; init; }
}


