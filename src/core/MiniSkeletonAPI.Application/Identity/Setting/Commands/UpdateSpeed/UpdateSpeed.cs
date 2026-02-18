using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataSpeeds.Commands.UpdateSpeed;

public record UpdateSpeedCommand : IRequest
{


    [JsonPropertyName("speed")]
    public float? Speed { get; set; }


}

public class UpdateSpeedCommandHandler : IRequestHandler<UpdateSpeedCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSpeedCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateSpeedCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.DataCounts.FirstOrDefault();

        if (entity != null) { 
                 entity.Speed =request.Speed;
                _context.DataCounts.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
         
       }
           
    }
}