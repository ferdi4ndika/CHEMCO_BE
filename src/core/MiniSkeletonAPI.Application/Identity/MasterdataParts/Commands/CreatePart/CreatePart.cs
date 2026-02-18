using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataParts.Commands.CreatePart;
public record CreatePartCommand : IRequest<Guid>
{
    [JsonPropertyName("part_name")]
    public string? PartName { get; init; }
    [JsonPropertyName("part_number")]
    public string? PartNUmber { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("qty")]
    public int? Qty { get; init; }
}


public class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreatePartCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var Part = new Part
        {

            PartName = request.PartName,
            Description = request.Description,
            PartNumber = request.PartNUmber,
            Qty = request.Qty
        };

         _context.Parts.Add(Part);
         await  _context.SaveChangesAsync(cancellationToken);
        return Part.Id;
    }
}
