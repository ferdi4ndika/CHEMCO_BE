using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMWarnas.Commands.UpdateMWarna;

public record UpdateMWarnaCommand : IRequest
{
    public required Guid Id { get; init; }
    [JsonPropertyName("coler")]
    public string? Coler { get; init; }
    [JsonPropertyName("description")]
    public string? Description { get; init; }

}

public class UpdateMWarnaCommandHandler : IRequestHandler<UpdateMWarnaCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMWarnaCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateMWarnaCommand request, CancellationToken cancellationToken)
    {
        var MWarna = new MWarna
        {
            Coler = request.Coler,
            Description = request.Description
        };

        var entity = _context.MWarnas.Find(request.Id);
        if (entity != null) { 
           entity.Coler = request.Coler;
            entity.Description = request.Description;
            _context.MWarnas.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
            
    }
}