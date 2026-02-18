using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataParts.Commands.UpdatePart;

public record UpdatePartCommand : IRequest
{
    public required Guid Id { get; init; }
    [JsonPropertyName("part_name")]
    public string? PartName { get; init; }
    [JsonPropertyName("part_number")]
    public string? PartNumber { get; init; }
    [JsonPropertyName("description")]

    public string? Description { get; init; }
    [JsonPropertyName("qty")]
    public int? Qty { get; init; }
}

public class UpdatePartCommandHandler : IRequestHandler<UpdatePartCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePartCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdatePartCommand request, CancellationToken cancellationToken)
    {
        var Part = new Part
        {
            PartName = request.PartName,
            Description = request.Description,
            PartNumber = request.PartNumber,
            Qty = request.Qty
        };
        var entity = _context.Parts.Find(request.Id);
        if (entity != null)
        {
            entity.PartName = request.PartName; 
            entity.PartNumber = request.PartNumber;
            entity.Description = request.Description;
            entity.Qty = request.Qty;
            _context.Parts.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
           
    }
}