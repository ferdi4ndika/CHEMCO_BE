using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;

public record DataAndonBriefDto
{
    public string Id { get; init; }
    [JsonPropertyName("part_name")]
    public string? PartName { get; init; }
    [JsonPropertyName("coler")]
    public string? Coler { get; init; }
    [JsonPropertyName("part_number")]
    public string? PartNumber { get; init; }
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    [JsonPropertyName("lot_material")]
    public string? LotMaterial { get; init; }

    [JsonPropertyName("repair")]
    public required string? Repair { get; init; }
    [JsonPropertyName("qty_part")]
    public int? qtypart { get; init; }
    [JsonPropertyName("qty_hanger")]
    public int? qtyhangar { get; init; }
    [JsonPropertyName("datetime")]
    public string? CreatedAt { get; set; }
    [JsonPropertyName("estimated_time")]
    public string? estimatedTime{ get; set; }
    public int? Status { get; set; }
}


