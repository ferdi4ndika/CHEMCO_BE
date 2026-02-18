using MiniSkeletonAPI.Domain.Entities;
using System.Text.Json.Serialization;


namespace MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
public record UserBriefDto
{
    [JsonPropertyName("id")]
    public string Id { get; init; }
    [JsonPropertyName("username")]
    public string UserName { get; init; }
    [JsonPropertyName("role")]

    public string Role { get; init; }
    [JsonPropertyName("permissions")]

    public string Permissions { get; init; }
    [JsonPropertyName("nik")]

    public string Nik { get; init; }
    [JsonPropertyName("datetime")]
    public DateTime Last_Created { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<User, UserBriefDto>();
    //    }
    //}
}

