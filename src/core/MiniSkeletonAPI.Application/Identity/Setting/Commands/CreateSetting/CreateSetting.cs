using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataSettings.Commands.CreateSetting;
public record CreateSettingCommand : IRequest<string>
{
    [JsonPropertyName("name")]

    public string? Name { get; set; }
    [JsonPropertyName("coler")]

    public string? Coler { get; set; }
    [JsonPropertyName("start_range")]
    public int? StartRage { get; set; }
    [JsonPropertyName("end_range")]
    public int? EndRage { get; set; }



}


public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, string>
{
    private readonly IApplicationDbContext _context;
    public CreateSettingCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        var Setting = new Setting
        {

           StartRage = request.StartRage,
           EndRage = request.EndRage,
           Name = request.Name,
           Coler = request.Coler,
        };

        if(request.StartRage > request.EndRage)
        {
            return "Invalid format";

        }
        //   var data = _context.Settings.Where(x =>
        //           x.StartRage <= request.StartRage &&
        //           x.EndRage >= request.StartRage
        //       ).ToList();
        //   var datas = _context.Settings.Where(x =>
        //    x.StartRage <= request.EndRage &&
        //    x.EndRage >= request.EndRage
        //).ToList();
        var data = _context.Settings
       .Where(x =>
           x.StartRage <= request.EndRage &&
           x.EndRage >= request.StartRage
       )
       .ToList();

        if (data.Count() != 0)
        {
            return "Data already exists";
        }
        _context.Settings.Add(Setting);
        await _context.SaveChangesAsync(cancellationToken);
        return Setting.Id.ToString();
    }
}
