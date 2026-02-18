using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Commands.UpdateDataAndon;

public record UpdateDataAndonCommand : IRequest
{
    public required Guid Id { get; init; }
    [JsonPropertyName("coler")]
    public required string? Coler { get; init; }
    [JsonPropertyName("id_part")]
    public required Guid? IdType { get; init; }
    [JsonPropertyName("description")]
    public required string? Description { get; init; }
    [JsonPropertyName("lot_material")]
    public required string? LotMaterial { get; init; }
    [JsonPropertyName("repair")]
    public required string? Repair { get; init; }
    [JsonPropertyName("qty_part")]
    public required int? QtyPart { get; init; }

}

public class UpdateDataAndonCommandHandler : IRequestHandler<UpdateDataAndonCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDataAndonCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateDataAndonCommand request, CancellationToken cancellationToken)
    {

        var entity = await _context.DataAndons.FindAsync(request.Id);
        if (entity != null)
        {
            entity.Coler = request.Coler;
            entity.IdType = request.IdType;
            entity.Repair = request.Repair;
            entity.Description = request.Description;
            entity.LotMaterial = request.LotMaterial;
            entity.QtyPart = request.QtyPart;

            _context.DataAndons.Update(entity);
          await _context.SaveChangesAsync(cancellationToken);
        }
    }
}