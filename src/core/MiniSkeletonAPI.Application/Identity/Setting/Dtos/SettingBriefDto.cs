using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Settings.Dtos;

public record SettingBriefDto
{
    public string Id { get; init; }
    [JsonPropertyName("name")]

    public string? Name { get; set; }
    [JsonPropertyName("coler")]

    public string? Coler { get; set; }
    [JsonPropertyName("start_range")]
    public int? StartRage { get; set; }
    [JsonPropertyName("end_range")]
    public int? EndRage { get; set; }
    [JsonPropertyName("datetime")]
    public string? CreatedAt { get; set; }


}
