using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Parts.Dtos;

public record PartBriefDto
{
    public string Id { get; init; }
    [JsonPropertyName("part_name")]
    public string? PartName { get; init; }
    [JsonPropertyName("part_number")]
    public string? PartNumber { get; init; }
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    [JsonPropertyName("qty")]
    public int? Qty { get; init; }

    [JsonPropertyName("datetime")]
    public string? CreatedAt { get; set; }
}
