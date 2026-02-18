using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;

public record DataAndonDetailBriefDto
{
    public Guid Id { get; init; }

    [JsonPropertyName("part_name")]
    public string? PartName { get; init; }
    [JsonPropertyName("coler")]
    public string? Coler { get; init; }
    [JsonPropertyName("part_number")]
    public string? PartNumber { get; init; }
    [JsonPropertyName("location")]
    public string? Locaton { get; init; }
    [JsonPropertyName("coler_location")]
    public string? ColerLocation { get; init; }

    [JsonPropertyName("no_hangar")]
    public int? LocatonNo { get; init; }
    [JsonPropertyName("qty_part")]
    public int? Qty { get; init; }
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    [JsonPropertyName("lot_material")]
    public string? LotMaterial { get; init; }
    [JsonPropertyName("datetime")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("repair")]
    public required string? Repair { get; init; }


}


