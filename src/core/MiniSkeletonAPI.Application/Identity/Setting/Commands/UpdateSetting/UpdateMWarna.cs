using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataSettings.Commands.UpdateSetting;

public record UpdateSettingCommand : IRequest
{
    public required Guid Id { get; init; }
    [JsonPropertyName("name")]


    public string? Name { get; set; }
    [JsonPropertyName("coler")]

    public string? Coler { get; set; }
    [JsonPropertyName("start_range")]
    public int? StartRage { get; set; }
    [JsonPropertyName("end_range")]
    public int? EndRage { get; set; }


}

public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSettingCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Settings.Find(request.Id);
        if (entity != null) { 
           entity.Name =request.Name;
            entity.Coler = request.Coler;
            entity.StartRage = request.StartRage;
            entity.EndRage = request.EndRage;
            var data = _context.Settings
            .Where(x =>
               x.StartRage <= request.EndRage &&
               x.EndRage >= request.StartRage && x.Id != request.Id
            )
            .ToList();

            if (data.Count() == 0 && request.StartRage < request.EndRage)
            {
                _context.Settings.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
           
       }
           
    }
}