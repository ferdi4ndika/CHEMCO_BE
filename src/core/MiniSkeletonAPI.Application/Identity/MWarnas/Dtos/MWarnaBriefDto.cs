using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MWarnas.Dtos;

public record MWarnaBriefDto
{
    public string Id { get; init; }
    [JsonPropertyName("coler")]
    public string? Coler { get; init; }
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("datetime")]
    public string? CreatedAt { get; set; }

}
