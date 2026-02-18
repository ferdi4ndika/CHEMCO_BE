using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Settings.Dtos;

public record SettingSpeedBriefDto
{


    [JsonPropertyName("speed")]
    public float? Speed { get; set; }
    //[JsonPropertyName("cound")]
    //public int? EndRage { get; set; }



}
