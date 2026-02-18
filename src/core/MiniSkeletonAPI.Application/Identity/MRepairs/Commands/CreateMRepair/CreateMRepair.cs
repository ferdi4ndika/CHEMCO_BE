using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMRepairs.Commands.CreateMRepair;
public record CreateMRepairCommand : IRequest<Guid>
{
    [JsonPropertyName("repair")]
    public string? Repair { get; init; }



}


public class CreateMRepairCommandHandler : IRequestHandler<CreateMRepairCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateMRepairCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMRepairCommand request, CancellationToken cancellationToken)
    {
        var MRepair = new MRepair
        {

            Repair = request.Repair
        };

         _context.MRepairs.Add(MRepair);
        await _context.SaveChangesAsync(cancellationToken);
        return MRepair.Id;
    }
}
