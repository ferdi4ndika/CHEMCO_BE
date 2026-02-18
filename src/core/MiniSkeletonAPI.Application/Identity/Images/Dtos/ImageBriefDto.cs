using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Images.Dtos;

public record ImageBriefDto
{
    public string Id { get; init; }
    //[JsonPropertyName("image")]
    public string? Name { get; init; }
    public string? Code { get; init; }
}
