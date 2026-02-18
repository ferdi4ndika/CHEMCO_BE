using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMWarnas.Commands.CreateMWarna;
public record CreateMWarnaCommand : IRequest<Guid>
{
    [JsonPropertyName("coler")]
    public string? Coler { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

}


public class CreateMWarnaCommandHandler : IRequestHandler<CreateMWarnaCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateMWarnaCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMWarnaCommand request, CancellationToken cancellationToken)
    {
        var MWarna = new MWarna
        {

            Coler = request.Coler,
            Description = request.Description
        };

         _context.MWarnas.Add(MWarna);
        await _context.SaveChangesAsync(cancellationToken);
        return MWarna.Id;
    }
}
