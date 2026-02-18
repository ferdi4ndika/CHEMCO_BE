using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MRepairs.Dtos;

public record MRepairBriefDto
{
    public string Id { get; init; }
    [JsonPropertyName("repair")]
    public string? Repair { get; init; }
    [JsonPropertyName("datetime")]
    public string? CreatedAt { get; set; }


}
